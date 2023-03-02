using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
    public Comment Comment { get; set; }

    public FileType FileType { get; set; }
}