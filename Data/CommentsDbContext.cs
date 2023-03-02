using LinqToDB.EntityFrameworkCore;

namespace TestAppDzenCode.Data;

using Microsoft.EntityFrameworkCore;

public class CommentsDbContext : DbContext    
{
    public DbSet<Comment> Comments { get; set; }

    public CommentsDbContext(DbContextOptions<CommentsDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.LazyLoadingEnabled = false;
    }
    
    public CommentsDbContext()
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.LazyLoadingEnabled = false;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DzenCodeConnectionString");
        optionsBuilder.UseNpgsql(connectionString);
    }
}