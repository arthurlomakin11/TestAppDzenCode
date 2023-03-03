using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAppDzenCode.Data;

namespace TestAppDzenCode.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ILogger<CommentsController> _logger;
    private readonly CommentsDbContext _commentsDbContext;

    public CommentsController(ILogger<CommentsController> logger, CommentsDbContext commentsDbContext)
    {
        _logger = logger;
        _commentsDbContext = commentsDbContext;
    }

    [HttpGet]
    public IEnumerable<Comment> Get(int skip)
    {
        var result = _commentsDbContext.Comments
            .Include(c => c.Files)
            .Where(c => c.Parent == null)
            .OrderBy(c => c.DateAdded)
            .Skip(skip * 25)
            .Take(25);

        return result;
    }
    
    [HttpGet("GetCommentsPagesNumber")]
    public double GetCommentsPagesNumber()
    {
        var result = _commentsDbContext.Comments.Count();

        var pagesCount = Math.Ceiling(result / 25.0);
        
        return pagesCount;
    }
    
    [HttpGet("GetCommentsTree")]
    public IEnumerable<Comment> GetCommentsTree(int Id)
    {
        var result = _commentsDbContext.Comments
            .Include(c => c.Files)
            .Where(c => c.Parent == null);

        return result;
    }
}