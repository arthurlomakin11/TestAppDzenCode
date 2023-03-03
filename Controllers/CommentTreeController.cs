using System.Linq;
using System.Text.Json.Serialization;
using LinqToDB;
using LinqToDB.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAppDzenCode.Controllers.Extensions;
using TestAppDzenCode.Data;

using File = TestAppDzenCode.Data.File;

namespace TestAppDzenCode.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentTreeController : ControllerBase
{
    private readonly ILogger<CommentTreeController> _logger;
    private readonly CommentsDbContext _commentsDbContext;
    private readonly IDataContext _commentsLinq2DbContext;

    public CommentTreeController(ILogger<CommentTreeController> logger, CommentsDbContext commentsDbContext)
    {
        _logger = logger;
        _commentsDbContext = commentsDbContext;
        _commentsLinq2DbContext = commentsDbContext.CreateLinqToDBContext();
    }
    
    [HttpGet]
    public IEnumerable<Comment> Get(int Id)
    {
        var commentsHierarchyCteAns = _commentsLinq2DbContext.GetCte<Comment>(commentHierarchy =>
        {
            return _commentsLinq2DbContext.GetTable<Comment>().Where(c => c.ParentId == Id)
                .Include(c => c.Comments)
                .Concat
                (
                    from c in _commentsLinq2DbContext.GetTable<Comment>()
                    from c2 in commentHierarchy.InnerJoin(eh => c.Id == eh.ParentId)
                    select new Comment
                    {
                        Id = c.Id,
                        ParentId = c.ParentId,
                        Email = c.Email,
                        UserName = c.UserName,
                        Text = c.Text,
                        DateAdded = c.DateAdded,
                        Files = new List<File> {
                            (from f in _commentsLinq2DbContext.GetTable<File>()
                                where f.CommentId == c.Id
                                select f).First()
                        }
                    }
                );
        });
        
        var commentsHierarchyCteDes = _commentsLinq2DbContext.GetCte<Comment>(commentHierarchy =>
        {
            return _commentsLinq2DbContext.GetTable<Comment>().Where(c => c.ParentId == Id)
                .Include(c => c.Comments)
                .Concat
                (
                    from c in _commentsLinq2DbContext.GetTable<Comment>()
                    from c2 in commentHierarchy.InnerJoin(eh => c.ParentId == eh.Id)
                    select new Comment
                    {
                        Id = c.Id,
                        ParentId = c.ParentId,
                        Email = c.Email,
                        UserName = c.UserName,
                        Text = c.Text,
                        DateAdded = c.DateAdded,
                        Files = new List<File> {
                            (from f in _commentsLinq2DbContext.GetTable<File>()
                            where f.CommentId == c.Id
                            select f).First()
                        }
                    }
                );
        });

        var result = commentsHierarchyCteAns.Union(commentsHierarchyCteDes);

        var resultTree = result
            .Include(c => c.Files)
            .ToList()
            .GenerateTree(c => c.Id, c => c.ParentId);

        return resultTree;
    }
}