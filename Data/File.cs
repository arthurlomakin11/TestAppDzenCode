using System.ComponentModel.DataAnnotations;

namespace TestAppDzenCode.Data;

public enum FileType
{
    Image,
    TextFile
}

public class File
{
    [Key]
    public int Id { get; set; }
    
    public string Src { get; set; }
    
    public int CommentId { get; set; }
    public Comment Comment { get; set; }

    public FileType FileType { get; set; }
}