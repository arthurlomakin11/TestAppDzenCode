using System.Drawing;
using TestAppDzenCode.Data;
using File = TestAppDzenCode.Data.File;

namespace TestAppDzenCode.Controllers.Extensions;

public class FileHandler
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileHandler(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public File SaveFile(IFormFile file)
    {
        var fileType = GetFileType(file);

        file = ProcessFile(file, fileType);

        var uniqueFileName = GetUniqueFileName(file.FileName);
        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        var filePath = Path.Combine(uploads, uniqueFileName);

        using var newFileStream = new FileStream(filePath, FileMode.Create);
        file.CopyTo(newFileStream);

        return new File
        {
            Src = Path.Combine("./uploads", uniqueFileName),
            FileType = fileType
        };
    }

    private static IFormFile ProcessFile(IFormFile file, FileType fileType)
    {
        switch (fileType)
        {
            case FileType.TextFile when file.Length >= 100000:
                throw new Exception("Text file size is bigger than 100KB");
            case FileType.Image:
            {
                var readStream = file.OpenReadStream();

                var img = Image.FromStream(readStream);
                var resizedImg = ImageManipulator.ScaleImage(img, 320, 240);

                var stream = new MemoryStream();
                resizedImg.Save(stream, img.RawFormat);
                stream.Position = 0;

                return new FormFile(stream, 0, stream.Length, file.Name, file.FileName);
            }
            default:
                return file;
        }
    }

    private static FileType GetFileType(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        if (extension is ".txt" or ".jpg" or ".gif" or ".png")
        {
            return extension switch
            {
                ".txt" => FileType.TextFile,
                ".jpg" or ".gif" or ".png" => FileType.Image,
                _ => throw new Exception("File type is not matching")
            };
        }
        
        throw new Exception("File type is not matching");
    }
    
    private static string GetUniqueFileName(string fileName)
    {
        fileName = Path.GetFileName(fileName);
        return Path.GetFileNameWithoutExtension(fileName)
                + "_" 
                + Guid.NewGuid().ToString()[..4] 
                + Path.GetExtension(fileName);
    }
}