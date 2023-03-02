using System.Collections;
using System.Text.Json;
using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
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
    private readonly DataConnection _commentsLinq2DbContext;

    public CommentsController(ILogger<CommentsController> logger, CommentsDbContext commentsDbContext)
    {
        _logger = logger;
        _commentsDbContext = commentsDbContext;
        _commentsLinq2DbContext = _commentsDbContext.CreateLinqToDBConnection();
    }

    [HttpGet]
    public IEnumerable<Comment> Get()
    {
        var commentsHierarchyCteAns = _commentsLinq2DbContext.GetCte<Comment>(commentHierarchy =>
        {
            return
                (
                    from e in _commentsLinq2DbContext.GetTable<Comment>()
                    select e
                )
                .Concat
                (
                    from c in _commentsLinq2DbContext.GetTable<Comment>()
                    from c2 in commentHierarchy
                        .InnerJoin(eh => c.Id == eh.ParentId)
                    select c
                );
        });
        
        var commentsHierarchyCteDes = _commentsLinq2DbContext.GetCte<Comment>(commentHierarchy =>
        {
            return
                (
                    from e in _commentsLinq2DbContext.GetTable<Comment>()
                    select e
                )
                .Concat
                (
                    from c in _commentsLinq2DbContext.GetTable<Comment>()
                    from c2 in commentHierarchy
                        .InnerJoin(eh => c.ParentId == eh.Id)
                    select c
                );
        });

        var result = commentsHierarchyCteAns.Union(commentsHierarchyCteDes);
            

        var x = result.ToList();
        /*foreach (Comment c in x)
        {
            var json = JsonSerializer.Serialize(c);
            Console.WriteLine(json);
        }
        */
        Hashtable
        

        var res = _commentsDbContext.Comments
            .Include(c => c.Files)
            .Include(c => c.Comments)
            .Where(c => c.Parent == null).ToQueryString();
        var res2 = _commentsDbContext.Comments.FromSqlRaw(res);

        return _commentsDbContext.Comments.Where(c => c.Parent == null);
    }
}