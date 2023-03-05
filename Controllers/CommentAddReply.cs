using Microsoft.AspNetCore.Mvc;
using TestAppDzenCode.Data;

namespace TestAppDzenCode.Controllers;

[ApiController]
[Route("api/comment/reply")]
public class CommentAddReply : ControllerBase
{
    private readonly ILogger<CommentAddReply> _logger;
    private readonly CommentsDbContext _commentsDbContext;

    public CommentAddReply(ILogger<CommentAddReply> logger, CommentsDbContext commentsDbContext)
    {
        _logger = logger;
        _commentsDbContext = commentsDbContext;
    }
    
    [HttpPost]
    public Comment ReplyToComment(string UserName, string Email, string Text, int RootCommentId)
    {
        var newComment = _commentsDbContext.Comments.Add(new Comment
        {
            ParentId = RootCommentId,
            Text = Text,
            UserName = UserName,
            Email = Email
        });
        _commentsDbContext.SaveChanges();
        return newComment.Entity;
    }
}