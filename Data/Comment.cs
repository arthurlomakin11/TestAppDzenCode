using System.ComponentModel.DataAnnotations;

namespace TestAppDzenCode.Data;

public class Comment
{
    [Key]
    public int Id { get; set; }
    
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Text { get; set; }
    
    public int? ParentId { get; set; }
    public Comment? Parent { get; set; }
    
    public List<File> Files { get; set; }
    public List<Comment> Comments { get; set; }
    
    public DateTime DateAdded { get; set; }
}