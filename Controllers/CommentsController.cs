using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAppDzenCode.Controllers.Extensions;
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

    public class OrderSelected
    {
        public int UserName { get; set; }
        public int Email { get; set; }
        public int DateAdded { get; set; }
    }

    [HttpGet]
    public IEnumerable<Comment> Get(int skip, int UserName, int Email, int DateAdded)
    {

        var orderSelected = new OrderSelected
        {
            UserName = UserName,
            Email = Email,
            DateAdded = DateAdded
        };

        var orderSelectedReflection = OrderSelectedReflectionTransformer.getOrderSelected(orderSelected);

        IQueryable<Comment> result = _commentsDbContext.Comments
            .Include(c => c.Files)
            .Where(c => c.Parent == null);

        Expression<Func<Comment, dynamic>> resultOrderPipeFunc = orderSelectedReflection.propertyName switch
        {
            "UserName" => c => c.UserName,
            "Email" => c => c.Email,
            "DateAdded" => c => c.DateAdded,
            _ => throw new Exception("NoPropertyToOrderError")
        };

        
        IQueryable<Comment> resultOrderPipe = orderSelectedReflection.orderType switch
        {
            OrderType.Asc => result.OrderBy(resultOrderPipeFunc),
            _ => result.OrderByDescending(resultOrderPipeFunc)
        };

        IQueryable<Comment> resultFilterPipe = resultOrderPipe.Skip(skip * 25).Take(25);

        return resultFilterPipe;
    }

    [HttpGet("GetCommentsPagesNumber")]
    public double GetCommentsPagesNumber()
    {
        var result = _commentsDbContext.Comments.Count();

        var pagesCount = Math.Ceiling(result / 25.0);
        
        return pagesCount;
    }
}