using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAppDzenCode.Data;

namespace TestAppDzenCode.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentTreeController : ControllerBase
{
    private readonly ILogger<CommentTreeController> _logger;
    private readonly CommentsDbContext _commentsDbContext;

    public CommentTreeController(ILogger<CommentTreeController> logger, CommentsDbContext commentsDbContext)
    {
        _logger = logger;
        _commentsDbContext = commentsDbContext;
    }

    [HttpGet()]
    public IEnumerable<Comment> Get(int Id)
    {
        var result = _commentsDbContext.Comments
            .Include(c => c.Files)
            .Include(c => c.Comments)
            .Where(c => c.Parent == null);

        return result;
    }
}