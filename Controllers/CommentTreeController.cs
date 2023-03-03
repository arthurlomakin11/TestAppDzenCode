using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAppDzenCode.Controllers.Extensions;
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

    [HttpGet]
    public IEnumerable<Comment> Get(int Id)
    {
        var result = _commentsDbContext.Comments
            .Include(c => c.Files)
            .Where(c => c.Id == 3 || c.Id == 4 || c.Id == 5)
            .ToList();

        return result.GenerateTree(c => c.Id, c => c.ParentId);
    }
}