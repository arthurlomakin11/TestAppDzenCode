using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestAppDzenCode.Data;

public class Comment
{
    [Key]
    public int Id { get; set; }
    
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Text { get; set; }
    
    public int? ParentId { get; set; }
    [JsonIgnore]
    public Comment? Parent { get; set; }
    
    public List<File> Files { get; set; }
    public IEnumerable<Comment> Comments { get; set; }
    
    public DateTime DateAdded { get; set; }

    public Comment()
    {
        DateAdded = DateTime.UtcNow;
    }
}