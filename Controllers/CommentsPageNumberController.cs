using Microsoft.AspNetCore.Mvc;
using TestAppDzenCode.Data;

namespace TestAppDzenCode.Controllers;

[ApiController]
[Route("api/getCommentsPagesNumber")]
public class CommentsPageNumberController : ControllerBase
{
    private readonly ILogger<CommentsPageNumberController> _logger;
    private readonly CommentsDbContext _commentsDbContext;

    public CommentsPageNumberController(ILogger<CommentsPageNumberController> logger, CommentsDbContext commentsDbContext)
    {
        _logger = logger;
        _commentsDbContext = commentsDbContext;
    }
    
    [HttpGet]
    public double GetCommentsPagesNumber()
    {
        var result = _commentsDbContext.Comments.Count();

        var pagesCount = Math.Ceiling(result / 25.0);
        
        return pagesCount;
    }
}