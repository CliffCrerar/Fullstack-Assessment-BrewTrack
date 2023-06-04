namespace BrewTrack.Services
{
    public static class WeatherServiceExtension
    {
        public static IServiceCollection AddWeatherService(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<WeatherService>();
            return services;
        }
    }
    public interface IWeatherService
    {

    }
    public class WeatherService: IWeatherService
    {
    }
}
