using BrewTrack.Data;
using BrewTrack.Dto;
using BrewTrack.Infra;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BrewTrack.Services
{
    public static class WeatherServiceExtension
    {
        public static IServiceCollection AddWeatherService(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IWeatherService,WeatherService>(serviceProvider =>
            {
                ConnectionMultiplexer redis = serviceProvider.GetService<ConnectionMultiplexer>();
                BrewTrackDbContext context = serviceProvider.GetService<BrewTrackDbContext>();
                string apiKey = config.GetValue<string>("WeatherApiKey");
                return new WeatherService(apiKey, context, redis);
            });
            return services;
        }
    }
    public interface IWeatherService
    {

    }
    public class WeatherService: IWeatherService
    {
        private readonly Weather _weatherApi;
        private readonly IDatabase _db;
        private readonly BrewTrackDbContext _dbContext;
        public WeatherService(string apiKey, BrewTrackDbContext dbContext, ConnectionMultiplexer redis)
        {
            _weatherApi = new Weather(apiKey);
            _db = redis.GetDatabase();
            _dbContext = dbContext;
        }

        private async Task<WeatherForeCastApiRequestDto> _getForCastFromApi(string longitude, string latitude)
        {
            return await _weatherApi.GetWeatherForLocation(latitude, longitude);
        }

        private void _saveWeatherData(WeatherForeCastApiRequestDto weatherData)
        {

        }
    }
}
