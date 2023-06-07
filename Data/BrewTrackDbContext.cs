using BrewTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace BrewTrack.Data;
public class BrewTrackDbContext : DbContext
{
    private IConfiguration _config;
    private ILogger<BrewTrackDbContext> _logger;
    public BrewTrackDbContext(DbContextOptions<BrewTrackDbContext> options) : base(options)
    {
        _logger = _getLogger();
        _config = new ConfigurationBuilder().Build();
        _logger.LogInformation("Executed BrewTrackDbContext Constructor with DbContext Options Params");
    }

    public BrewTrackDbContext()
    {
        _logger = _getLogger();
        _config = new ConfigurationBuilder().Build();
        _logger.LogInformation("Executed BrewTrackDbContext Paramless Constructor");
    }

    private ILogger<BrewTrackDbContext> _getLogger()
    {
        var loggerFac = new LoggerFactory();
        var logger = loggerFac.CreateLogger<BrewTrackDbContext>();
        loggerFac.Dispose();
        return logger;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _logger.LogInformation("Executing On Model Create.");
        modelBuilder.Entity<ApiSource>().HasData(
            new ApiSource
            {
                ApiSourceName = "Weather",
            },
            new ApiSource
            {
                ApiSourceName = BrewTrackContstants.BrewerySourceKey,
            }
        );
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserHistory> UserHistory => Set<UserHistory>();
    public DbSet<BrewPub> Brewpubs => Set<BrewPub>();
    public DbSet<ApiSource> ApiSources => Set<ApiSource>();
    public DbSet<CachedTimeline> CachedTimeline => Set<CachedTimeline>();
    public DbSet<WeatherForeCastHeader> WeatherForeCastHeaders => Set<WeatherForeCastHeader>();
    public DbSet<WeatherForeCastDetails> WeatherForeCastDetails => Set<WeatherForeCastDetails>();
}
