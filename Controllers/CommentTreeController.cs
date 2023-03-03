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
            return (
                    from c in _commentsLinq2DbContext.GetTable<Comment>()
                    let json = EF.Functions.JsonAgg(_commentsLinq2DbContext.GetTable<File>())
                    select new Comment
                    {
                        Id = c.Id, 
                        UserName = c.UserName,
                        Email = c.Email,
                        Text = c.Text,
                        ParentId = c.ParentId,
                        Files = c.Files,
                        DateAdded = c.DateAdded,
                        JsonFiles = json
                    }
                )
                .Concat
                (
                    from c in _commentsLinq2DbContext.GetTable<Comment>()
                    from c2 in commentHierarchy.InnerJoin(eh => c.Id == eh.ParentId)
                    let json = EF.Functions.JsonAgg(_commentsLinq2DbContext.GetTable<File>())
                    select new Comment
                    {
                        Id = c.Id, 
                        UserName = c.UserName,
                        Email = c.Email,
                        Text = c.Text,
                        ParentId = c.ParentId,
                        Files = c.Files,
                        DateAdded = c.DateAdded,
                        JsonFiles = json
                    }
                );
        });
        
        var commentsHierarchyCteDes = _commentsLinq2DbContext.GetCte<Comment>(commentHierarchy =>
        {
            return (
                    from c in _commentsLinq2DbContext.GetTable<Comment>()
                    let json = EF.Functions.JsonAgg(_commentsLinq2DbContext.GetTable<File>())
                    select new Comment
                    {
                        Id = c.Id, 
                        UserName = c.UserName,
                        Email = c.Email,
                        Text = c.Text,
                        ParentId = c.ParentId,
                        Files = c.Files,
                        DateAdded = c.DateAdded,
                        JsonFiles = json
                    }
                )
                .Concat
                (
                    from c in _commentsLinq2DbContext.GetTable<Comment>()
                    from c2 in commentHierarchy.InnerJoin(eh => c.ParentId == eh.Id)
                    let json = EF.Functions.JsonAgg(_commentsLinq2DbContext.GetTable<File>())
                    select new Comment
                    {
                        Id = c.Id, 
                        UserName = c.UserName,
                        Email = c.Email,
                        Text = c.Text,
                        ParentId = c.ParentId,
                        Files = c.Files,
                        DateAdded = c.DateAdded,
                        JsonFiles = json
                    }
                );
        });

        var result = commentsHierarchyCteAns.Union(commentsHierarchyCteDes);
        
        
        var resultTree = result.ToList().GenerateTree(c => c.Id, c => c.ParentId);

        return resultTree;
    }
}