using Microsoft.AspNetCore.Mvc;
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
    public IEnumerable<Comment> Get()
    {
        return _commentsDbContext.Comments.Where(c => c.Parent == null);
    }
}