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
            services.AddScoped<IWeatherService,WeatherService>(serviceProvider =>
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(config.GetConnectionString("Redis"));
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
        Task<TransformedWeatherDto> GetWeatherForecast(string latitude, string longitude);
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
            _logger = logger;
            _sourceKey = BrewTrackContstants.WeatherSourceKey;
            _weatherApi = new Weather(apiKey);
            _dbContext = dbContext;
            _db = redis.GetDatabase();
            _cacheDate = _getCacheDate();
            _apiSourceRefId = _getApiSourceRefId();

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
            try
            {
                return _dbContext.CachedTimeline
                    .Where(row => row.ApiSourceRefId == _apiSourceRefId)
                    .OrderBy(row => row.Date)
                    .Select(row => row.Date)
                    .Last();
            }
            catch( Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return DateTime.MinValue;
            }
            

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
        public async Task<TransformedWeatherDto> GetWeatherForecast(string latitude, string longitude)
        {

            WeatherForeCastApiRequestDto weatherForecast = await _getForecastFromApi(latitude, longitude);
            
            var tempratureTimeDays = weatherForecast.Hours
                .Select(row =>
                {
                    var parsedDate = DateTime.Parse(row.Time.Replace(" ", ""));
                    return new {
                        Day = parsedDate.Day,
                        Month = parsedDate.Month,
                        Time = parsedDate.TimeOfDay,
                        FullDate = parsedDate,
                        AirTemperature = row.AirTemperature
                    };
                })
                .ToList();

            var groupByDays = from timeEntry in tempratureTimeDays group timeEntry by timeEntry.Day into days select days;

            TransformedWeatherDto foreCast = new()
            {
                Meta = weatherForecast.Meta
            };

            foreach (var day in groupByDays)
            {
                var groupArray = day.ToArray();
                var dayEntry = new TemperaturesPerDay()
                {
                    Day = groupArray[0].Day,
                    FullDate = groupArray[0].FullDate.Date
                };
                foreach (var entry in day)
                {
                    Console.WriteLine(entry.AirTemperature);
                    TemperaturePerHour timeEntry = new()
                    {
                        Hour = entry.Time.Hours,
                        Time = entry.FullDate.TimeOfDay,
                        FullDate = entry.FullDate,
                        AirTemperature = entry.AirTemperature.Noaa
                    };
                    dayEntry.Temperatures.Add(timeEntry);
                }
                dayEntry.AverageTemperature = dayEntry.Temperatures.Select(row => row.AirTemperature).Average();
                foreCast.TemperaturesPerDays.Add(dayEntry);
            }


                return foreCast;
            //    new WeatherForeCastApiRequestDto
            //{
            //    Hours = weatherForecast.Hours,
            //    Meta = weatherForecast.Meta
            //};
        }
    }
}
