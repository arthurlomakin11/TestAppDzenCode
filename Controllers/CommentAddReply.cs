using System.ComponentModel.DataAnnotations;
using Ganss.Xss;
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
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CommentAddReply(ILogger<CommentAddReply> logger, CommentsDbContext commentsDbContext, ReCaptcha reCaptcha, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _commentsDbContext = commentsDbContext;
        _reCaptcha = reCaptcha;
        _webHostEnvironment = webHostEnvironment;
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
    public async Task<ActionResult<Comment>> ReplyToComment([FromForm] ReplyToCommentBody body, [FromForm(Name = "Files[]")] IFormFileCollection files)
    {
        var reCaptchaTokenValid = await _reCaptcha.IsValid(body.Token);
        if (!reCaptchaTokenValid)
        {
            return new ActionResult<Comment>(BadRequest("ReCaptcha Token Validation Error"));
        }

        var filesLoaded = from file in files
            let fileHandler = new FileHandler(_webHostEnvironment)
            select fileHandler.SaveFile(file);

        var newComment = _commentsDbContext.Comments.Add(new Comment
        {
            ParentId = body.RootCommentId,
            Text = SanitizeHtmlText(body.Text),
            UserName = body.UserName,
            Email = body.Email,
            Files = filesLoaded.ToList()
        });
        await _commentsDbContext.SaveChangesAsync();
        
        return newComment.Entity;
    }

    public string SanitizeHtmlText(string text)
    {
        var sanitizer = new HtmlSanitizer(new HtmlSanitizerOptions
        {
            AllowedTags = new HashSet<string> {"i", "strong", "a", "code" },
            AllowedAttributes = new SortedSet<string> { "href" }
        });

        var newText = sanitizer.Sanitize(text);
        
        return newText;
    }
}