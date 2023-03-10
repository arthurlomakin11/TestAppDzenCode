using System.Linq.Expressions;
using LinqToDB;
using LinqToDB.EntityFrameworkCore;
using LinqToDB.Expressions;
using LinqToDB.Linq;
using Microsoft.AspNetCore.Mvc;
using TestAppDzenCode.Controllers.Extensions;
using TestAppDzenCode.Data;

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
        Func<Comment, Comment, Expression<Func<bool>>> cteConcatBlockJoinPredicateAns = (c, eh) =>
            () => c.Id == eh.ParentId;

        Func<Comment, Comment, Expression<Func<bool>>> cteConcatBlockJoinPredicateDes = (c, eh) =>
            () => c.ParentId == eh.Id;
        
        var commentsHierarchyCteAns = generateCte(Id, cteConcatBlockJoinPredicateAns);
        var commentsHierarchyCteDes = generateCte(Id, cteConcatBlockJoinPredicateDes);

        var resultCteUnion = commentsHierarchyCteAns.Union(commentsHierarchyCteDes);

        // If there is no children, result is empty, so add the root
        var rootSelector = _commentsLinq2DbContext.GetTable<Comment>().Where(c => c.Id == Id);
        var result = resultCteUnion.Union(rootSelector);
        
        var resultTree = result
            .LoadWith(c => c.Files)
            .ToList()
            .GenerateTree(c => c.Id, c => c.ParentId);

        return resultTree;
    }
    
    
    private IQueryable<Comment> generateCte(int Id, Func<Comment, Comment, Expression<Func<bool>>> cteConcatBlockJoinPredicate)
    {
        return _commentsLinq2DbContext.GetCte<Comment>(commentHierarchy =>
        {
            return _commentsLinq2DbContext.GetTable<Comment>()
                .Where(c => c.ParentId == Id)
                .Concat
                (
                    from c in _commentsLinq2DbContext.GetTable<Comment>()
                    from c2 in commentHierarchy.InnerJoin(eh => cteConcatBlockJoinPredicate(eh, c).Compile()())
                    select c
                );
        });
    }
}