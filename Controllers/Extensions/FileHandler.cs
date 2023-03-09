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