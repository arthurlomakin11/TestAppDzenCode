using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TestAppDzenCode.Controllers.Extensions;
using TestAppDzenCode.Data;

namespace TestAppDzenCode.Controllers;

[ApiController]
[Route("api/comment/reply")]
public class CommentAddReply : ControllerBase
{
    private readonly ILogger<CommentAddReply> _logger;
    private readonly CommentsDbContext _commentsDbContext;
    private readonly ReCaptcha _reCaptcha;

    public CommentAddReply(ILogger<CommentAddReply> logger, CommentsDbContext commentsDbContext, ReCaptcha reCaptcha)
    {
        _logger = logger;
        _commentsDbContext = commentsDbContext;
        _reCaptcha = reCaptcha;
    }

    public class ReplyToCommentBody
    {
        [RegularExpression("^[a-zA-Z0-9]*$")]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Text { get; set; }
        public int RootCommentId { get; set; }

        public string Token { get; set; }
    }
    
    [HttpPost]
    public async Task<ActionResult<Comment>> ReplyToComment([FromBody] ReplyToCommentBody Body)
    {
        var reCaptchaTokenValid = await _reCaptcha.IsValid(Body.Token);
        if (!reCaptchaTokenValid)
        {
            return new ActionResult<Comment>(BadRequest("ReCaptcha Token Validation Error"));
        }
        
        var newComment = _commentsDbContext.Comments.Add(new Comment
        {
            ParentId = Body.RootCommentId,
            Text = Body.Text,
            UserName = Body.UserName,
            Email = Body.Email
        });
        await _commentsDbContext.SaveChangesAsync();
        return newComment.Entity;
    }
}