using StackExchange.Redis;
using BrewTrack.Helpers;
using BrewTrack.Models;
using BrewTrack.Infra;
using BrewTrack.Data;
using BrewTrack.Dto;
using Newtonsoft.Json;
using System.Data;
using Microsoft.EntityFrameworkCore;
using MySql;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BrewTrack.Services
{
    /// <summary>
    /// Weather service extension
    /// </summary>
    public static class WeatherServiceExtension
    {
        /// <summary>
        /// Add Weather service to an existing service collection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddWeatherService(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IWeatherService, WeatherService>(serviceProvider =>
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
    /// <summary>
    /// Weather service interface
    /// </summary>
    public interface IWeatherService
    {
        Task<TransformedWeatherDto> GetWeatherForecast(string latitude, string longitude);
    }
    /// <summary>
    /// Weather service
    /// </summary>
    public class WeatherService : IWeatherService
    {
        private readonly ILogger<WeatherService> _logger;
        private readonly BrewTrackDbContext _dbContext;
        private readonly Guid _apiSourceRefId;
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
            _apiSourceRefId = _getApiSourceRefId();
        }

        // get api source ref by looking up source api name
        private Guid _getApiSourceRefId()
        {
            return _dbContext.ApiSources
                .Where(row => row.ApiSourceName == _sourceKey)
                .Select(row => row.Id)
                .FirstOrDefault();
        }
        // serialize
        private string _serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        // deserialize
        private T _deserialize<T>(string json)
        {
            return Ensure.ArgumentNotNull(JsonConvert.DeserializeObject<T>(json));
        }
        // get from api
        private async Task<TransformedWeatherDto> _getForecastFromApi(string latitude, string longitude)
        {
            WeatherForeCastApiRequestDto apiWeatherData = await _weatherApi.GetWeatherForLocation(latitude, longitude);
            return _transformData(apiWeatherData);
        }
        // transform api data
        private TransformedWeatherDto _transformData(WeatherForeCastApiRequestDto weatherForcastApiData)
        {
            var tempratureTimeDays = weatherForcastApiData.Hours
                .Select(row =>
                {
                    var parsedDate = DateTime.Parse(row.Time.Replace(" ", ""));
                    return new
                    {
                        Day = parsedDate.Day,
                        Month = parsedDate.Month,
                        Time = parsedDate.TimeOfDay,
                        FullDate = parsedDate,
                        AirTemperature = row.AirTemperature
                    };
                })
                .ToList();

            var groupByDays = from timeEntry in tempratureTimeDays group timeEntry by timeEntry.Day into groupDay select groupDay;

            IList<WeatherForecastDay> days = new List<WeatherForecastDay>();

            foreach (var day in groupByDays)
            {
                var groupArray = day.ToArray();
                WeatherForecastDay dayEntry = new()
                {
                    Day = groupArray[0].Day,
                    FullDate = groupArray[0].FullDate.Date
                };
                foreach (var entry in day)
                {
                    Console.WriteLine(entry.AirTemperature);
                    WeatherForecastHour timeEntry = new()
                    {
                        Hour = entry.Time.Hours,
                        Time = entry.FullDate.TimeOfDay,
                        FullDate = entry.FullDate,
                        AirTemperature = entry.AirTemperature.Noaa
                    };
                    dayEntry.Temperatures.Add(timeEntry);
                }
                dayEntry.AverageTemperature = dayEntry.Temperatures.Select(row => row.AirTemperature).Average();
                days.Add(dayEntry);
            }
            return new()
            {
                Meta = weatherForcastApiData.Meta,
                TemperaturesPerDays = days
            };
        }

        // check if requested co-ords are in redis
        private bool _coordinatesInCache(string latitude, string longitude)
        {
            return _db.KeyExists("LAT_" + latitude + "_LNG_" + longitude);
        }

        /// <summary>
        /// Get the weather forecast for the specified Longitude and Latitude as parameters
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public async Task<TransformedWeatherDto> GetWeatherForecast(string latitude, string longitude)
        {
            try
            {
                TransformedWeatherDto weatherForecast;
                if (_coordinatesInCache(latitude, longitude))
                {
                    var stringData = _db.StringGet("LAT_" + latitude + "_LNG_" + longitude);
                    weatherForecast = _deserialize<TransformedWeatherDto>(Ensure.ArgumentNotNull(stringData.ToString()));
                } else
                {
                    weatherForecast = await _getForecastFromApi(latitude, longitude);
                    _db.StringSet("LAT_" + latitude + "_LNG_" + longitude, _serialize(weatherForecast));
                }
                return weatherForecast;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred in weather forecasting service");
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
