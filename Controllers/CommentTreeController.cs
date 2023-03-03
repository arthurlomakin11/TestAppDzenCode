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
        var fileCte = (from f in _commentsLinq2DbContext.GetTable<File>()
            select f).AsCte();
        
        var commentsHierarchyCteAns = _commentsLinq2DbContext.GetCte<Comment>(commentHierarchy =>
        {
            return
                (
                    from c in _commentsLinq2DbContext.GetTable<Comment>()
                    select new Comment
                    {
                        Id = c.Id,
                        ParentId = c.ParentId,
                        Email = c.Email,
                        UserName = c.UserName,
                        Text = c.Text,
                        DateAdded = c.DateAdded,
                        Files = new List<File>(fileCte.Where(f => f.CommentId == c.Id))
                    }
                )
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
                        Files = new List<File>(fileCte.Where(f => f.CommentId == c.Id))
                    }
                );
        });
        
        var commentsHierarchyCteDes = _commentsLinq2DbContext.GetCte<Comment>(commentHierarchy =>
        {
            return
                (
                    from c in _commentsLinq2DbContext.GetTable<Comment>()
                    select new Comment
                    {
                        Id = c.Id,
                        ParentId = c.ParentId,
                        Email = c.Email,
                        UserName = c.UserName,
                        Text = c.Text,
                        DateAdded = c.DateAdded,
                        Files = new List<File>(fileCte.Where(f => f.CommentId == c.Id))
                    }
                )
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
                        Files = new List<File>(fileCte.Where(f => f.CommentId == c.Id))
                    }
                );
        });

        var x = commentsHierarchyCteAns.Union(commentsHierarchyCteDes)
            .Where(c => c.Id == 3 || c.Id == 4 || c.Id == 5);
        
        Console.WriteLine(x.ToQueryString());

        x.ToList();
        
        var result = _commentsDbContext.Comments
            .Include(c => c.Files)
            .Where(c => c.Id == 3 || c.Id == 4 || c.Id == 5)
            .ToList();

        return result.GenerateTree(c => c.Id, c => c.ParentId);
    }
}