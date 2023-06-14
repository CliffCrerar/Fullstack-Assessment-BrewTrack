using System.ComponentModel.Design;

namespace BrewTrack.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBrewTrackService(this IServiceCollection services, IConfiguration config) {

            return services
                .AddBreweriesService(config)
                .AddUserService()
                .AddWeatherService(config);
        }
    }
}
