using BrewTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace BrewTrack.Data;
public class BrewTrackDbContext : DbContext
{
    private IConfiguration _config;
    public BrewTrackDbContext(DbContextOptions<BrewTrackDbContext> options) : base(options)
    {
        _config = new ConfigurationBuilder().Build();
    }

    public BrewTrackDbContext()
    {
        _config = new ConfigurationBuilder().Build();
    }

    public void OnModelCreate(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiSource>().HasData(
            new ApiSource
            {
                Id = new Guid(),
                ApiSourceName = "Waether",
                DateCreated = DateTime.UtcNow
            },
            new ApiSource
            {
                Id = new Guid(),
                ApiSourceName = "Brewery",
                DateCreated = DateTime.UtcNow
            }
        );
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder options)
    //    => options.UseMySQL(_config.GetConnectionString("MySql"));

    public DbSet<User> Users => Set<User>();
    public DbSet<UserHistory> UserHistory => Set<UserHistory>();
    public DbSet<BrewPub> Brewpubs => Set<BrewPub>();
    public DbSet<ApiSource> ApiSources => Set<ApiSource>();
    public DbSet<CachedTimeline> CachedTimeline => Set<CachedTimeline>();
}
