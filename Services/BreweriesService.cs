using BrewTrack.Data;

namespace BrewTrack.Services
{
    public static class BreweriesServiceExtension 
    {
        public static IServiceCollection AddBReweriesService(this IServiceCollection services, IConfiguration config) 
        {
            services.AddTransient<IBreweriesService>(provider =>
            {
                BrewTrackDbContext context = provider.GetService<BrewTrackDbContext>();
                return new BreweriesService(context);
            });
            return services;
        }
    }
    public interface IBreweriesService
    {

    }
    public class BreweriesService: IBreweriesService
    {
        public BreweriesService(BrewTrackDbContext dbContext)
        {

        }
    }
}
