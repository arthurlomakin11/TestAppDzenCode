using LinqToDB.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TestAppDzenCode.Controllers.Extensions;
using TestAppDzenCode.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews()
    .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

var connectionString = builder.Configuration.GetConnectionString("DzenCodeConnectionString");
builder.Services.AddDbContext<CommentsDbContext>(o =>o.UseNpgsql(connectionString));
LinqToDBForEFTools.Initialize();

builder.Services.AddHttpClient<ReCaptcha>(x =>
{
    x.BaseAddress = new Uri("https://www.google.com/recaptcha/api/siteverify");
});

var app = builder.Build();

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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
;

app.Run();