using System.Text.Json.Nodes;

namespace TestAppDzenCode.Controllers.Extensions;

public class ReCaptcha
{
    private readonly HttpClient captchaClient;

    public ReCaptcha(HttpClient captchaClient)
    {
        this.captchaClient = captchaClient;
    }

    private const string ReCaptchaSecret = "6LeQYdckAAAAALr4XgsCf73McwfLPZibWZ3Se0zo";

    public async Task<bool> IsValid(string captcha)
    {
        var postTask = await captchaClient.PostAsync($"?secret={ReCaptchaSecret}&response={captcha}", new StringContent(""));
        var result = await postTask.Content.ReadAsStringAsync();
        var resultObject = JsonNode.Parse(result);
        dynamic success = resultObject["success"];
        return (bool)success;
    }
}