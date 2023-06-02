using BrewTrack.Data;
using BrewTrack.Helpers;
using BrewTrack.Infra;
using BrewTrack.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace BrewTrack.Services
{
    public static class BreweriesServiceExtension
    {
        public static IServiceCollection AddBreweriesService(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IBreweriesService, BreweriesService>(provider =>
            {
                BrewTrackDbContext context = Ensure.ArgumentNotNull(provider.GetService<BrewTrackDbContext>());
                IConnectionMultiplexer redis = Ensure.ArgumentNotNull(provider.GetService<IConnectionMultiplexer>());
                ILogger<BreweriesService> logger = Ensure.ArgumentNotNull(provider.GetService<ILogger<BreweriesService>>());
                return new BreweriesService(context, redis, logger);
            });
            return services;
        }
    }
    public interface IBreweriesService
    {
        IList<BrewPub> GetData();
    }
    public class BreweriesService : IBreweriesService
    {
        private readonly BrewTrackDbContext _dbContext;
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<BreweriesService> _logger;
        private readonly IDatabase _db;
        private DateTime _cacheDate => _getCacheDate();
        public BreweriesService(BrewTrackDbContext dbContext, IConnectionMultiplexer redis, ILogger<BreweriesService> logger)
        {
            _dbContext = dbContext;
            _redis = redis;
            _logger = logger;
            _db = _redis.GetDatabase();
        }

        private DateTime _getCacheDate()
        {
            DateTime cacheDate = _dbContext.CachedTimeline.Select(x => x.Date).FirstOrDefault();
            DateTime timeStamp = DateTime.UtcNow;
            bool isNullOrEmptyDate = string.IsNullOrEmpty(cacheDate.ToString());
            return isNullOrEmptyDate ? timeStamp : cacheDate;
        }

        private void _getBreweriesData()
        {
            if (!_db.KeyExists("Breweries"))
            {
                var data = Breweries.GetData().Result;
                IDatabase db = _redis.GetDatabase();
                db.StringSet("Breweries", JsonSerializer.Serialize(data));
            }
        }

        public IList<BrewPub> GetData()
        {
            _logger.LogInformation("Service BreweriesService, running GetData Method");
            IList<BrewPub> data;
            if (!_db.KeyExists("Breweries"))
            {
                data = Breweries.GetData().Result;
                _db.StringSet("Breweries", JsonSerializer.Serialize(data));
                return data;
            }
            var redisData = Ensure.ArgumentNotNull(_db.StringGet("Breweries").ToString());
            return Ensure.ArgumentNotNull(JsonSerializer.Deserialize<BrewPub[]>(redisData));
        }
    }
}
