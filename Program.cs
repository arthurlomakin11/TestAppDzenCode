using LinqToDB.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TestAppDzenCode.Controllers.Extensions;
using TestAppDzenCode.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNamingPolicy = null;
        opts.JsonSerializerOptions.MaxDepth = 300;
    });

var connectionString = builder.Configuration.GetConnectionString("DzenCodeConnectionString");
builder.Services.AddDbContext<CommentsDbContext>(o => o.UseNpgsql(connectionString));
LinqToDBForEFTools.Initialize();
LinqToDB.Data.DataConnection.TurnTraceSwitchOn();
LinqToDB.Data.DataConnection.WriteTraceLine = (message, displayName, Level) => { Console.WriteLine($"{message} {displayName}"); };
LinqToDB.Common.Configuration.Linq.GenerateExpressionTest = true;

builder.Services.AddHttpClient<ReCaptcha>(x =>
{
    x.BaseAddress = new Uri("https://www.google.com/recaptcha/api/siteverify");
});

var app = builder.Build();

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<CommentsDbContext>();
    context.Database.Migrate();
}

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;
    var response = new { error = exception.Message };
    await context.Response.WriteAsJsonAsync(response);
}));

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = context =>
        context.Context.Response.Headers.Add("Cache-Control", "no-cache")
});
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();