using BrewTrack.Data;
using BrewTrack.Helpers;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BrewTrack.Services
{
    public static class BreweriesServiceExtension 
    {
        public static IServiceCollection AddBreweriesService(this IServiceCollection services, IConfiguration config) 
        {
            services.AddScoped<IBreweriesService,BreweriesService>(provider =>
            {
                BrewTrackDbContext context = Ensure.ArgumentNotNull( provider.GetService<BrewTrackDbContext>());
                IConnectionMultiplexer redis = Ensure.ArgumentNotNull(provider.GetService<IConnectionMultiplexer>());
                return new BreweriesService(context, redis);
            });
            return services;
        }
    }
    public interface IBreweriesService
    {

    }
    public class BreweriesService: IBreweriesService
    {
        private readonly BrewTrackDbContext _dbContext;
        private readonly IConnectionMultiplexer _redis;
        public BreweriesService(BrewTrackDbContext dbContext, IConnectionMultiplexer redis)
        {
            _dbContext = dbContext;
            _redis = redis;
        }


    }
}
