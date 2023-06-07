using StackExchange.Redis;
using BrewTrack.Helpers;
using BrewTrack.Models;
using BrewTrack.Infra;
using BrewTrack.Data;
using BrewTrack.Dto;
using Newtonsoft.Json;

namespace BrewTrack.Services
{
    public static class WeatherServiceExtension
    {
        public static IServiceCollection AddWeatherService(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IWeatherService,WeatherService>(serviceProvider =>
            {
                ConnectionMultiplexer redis = _notNull(serviceProvider.GetService<ConnectionMultiplexer>());
                BrewTrackDbContext context = _notNull(serviceProvider.GetService<BrewTrackDbContext>());
                ILogger<WeatherService> logger = _notNull(serviceProvider.GetService<ILogger<WeatherService>>());
                string apiKey = config.GetValue<string>("WeatherApiKey");
                return new WeatherService(apiKey, context, redis, logger);
            });
            return services;
        }

        private static T _notNull<T>(T? arg)
        {
            return Ensure.ArgumentNotNull<T>(arg);
        }
    }
    public interface IWeatherService
    {
        Task<WeatherForeCastApiRequestDto> GetWeatherForecast(string latitude, string longitude);
    }
    public class WeatherService: IWeatherService
    {
        private readonly ILogger<WeatherService> _logger;
        private readonly BrewTrackDbContext _dbContext;
        private readonly Guid _apiSourceRefId;
        private readonly DateTime _cacheDate;
        private readonly Weather _weatherApi;
        private readonly string _sourceKey;
        private readonly IDatabase _db;
        public WeatherService(string apiKey, BrewTrackDbContext dbContext, ConnectionMultiplexer redis, ILogger<WeatherService> logger)
        {
            _sourceKey = BrewTrackContstants.WeatherSourceKey;
            _apiSourceRefId = _getApiSourceRefId();
            _weatherApi = new Weather(apiKey);
            _cacheDate = _getCacheDate();
            _db = redis.GetDatabase();
            _dbContext = dbContext;
            _logger = logger;
        }

        private Guid _getApiSourceRefId()
        {
            return _dbContext.ApiSources
                .Where(row => row.ApiSourceName == _sourceKey)
                .Select(row => row.Id)
                .FirstOrDefault();
        }

        public DateTime _getCacheDate()
        {
            
            return _dbContext.CachedTimeline
                .Where(row => row.ApiSourceRefId == _apiSourceRefId)
                .OrderBy(row => row.Date)
                .Select(row => row.Date)
                .Last();
        }
        private async void _setCacheDate()
        {
            try
            {
                CachedTimeline cacheTimeLine = new CachedTimeline
                {
                    ApiSourceRefId = _apiSourceRefId
                };
                _dbContext.CachedTimeline.Add(cacheTimeLine);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogInformation("Error occured while saving cacheDate");
                _logger.LogError(ex.Message, ex);
                throw; 
            }
        }

        private string _serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        private T _deserialize<T>(string json)
        {
            return Ensure.ArgumentNotNull(JsonConvert.DeserializeObject<T>(json));
        }

        private bool _keyExist(string key)
        {
            return _db.KeyExists(key);
        }

        private T _getFromCache<T>(string key)
        {
            return _deserialize<T>(_db.StringGet(key));
        }
        private void _saveInCache<T>(string key, T obj)
        {
            _db.StringSet(key,_serialize<T>(obj));
        }
        private async Task<WeatherForeCastApiRequestDto> _getForecastFromApi(string latitude, string longitude)
        {
            return await _weatherApi.GetWeatherForLocation(latitude, longitude);
        }
        private void _saveWeatherDataHeader(WeatherForeCastApiRequestDto forecastData)
        {

        }
        private void _saveWeatherDataDetails(WeatherForeCastApiRequestDto forecastData)
        {

        }
        private void _saveWeatherData(WeatherForeCastApiRequestDto weatherData)
        {

        }
        public async Task<WeatherForeCastApiRequestDto> GetWeatherForecast(string latitude, string longitude)
        {
            return await _getForecastFromApi(latitude, longitude);
        }
    }
}
