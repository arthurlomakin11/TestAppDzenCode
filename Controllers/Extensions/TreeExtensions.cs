using TestAppDzenCode.Data;

namespace TestAppDzenCode.Controllers.Extensions;


static class TreeExtensions
{
    public static IEnumerable<Comment> GenerateTree(
        this IEnumerable<Comment> collection,
        Func<Comment, int> idSelector,
        Func<Comment, int?> parentIdSelector,
        int? rootId = null)
    {
        return collection.Where(c => parentIdSelector(c) == rootId)
            .Select(c => new Comment()
            {
                Id = c.Id, 
                UserName = c.UserName,
                Email = c.Email,
                Text = c.Text,
                ParentId = c.ParentId,
                Files = c.Files,
                DateAdded = c.DateAdded,
                Comments = collection.GenerateTree(idSelector, parentIdSelector, idSelector(c))
            });
    }
}