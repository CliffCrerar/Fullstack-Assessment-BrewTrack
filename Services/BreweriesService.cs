using BrewTrack.Data;

namespace BrewTrack.Services
{
    public static class BreweriesServiceExtension 
    {
        public static IServiceCollection AddBreweriesService(this IServiceCollection services, IConfiguration config) 
        {
            services.AddScoped<IBreweriesService,BreweriesService>(provider =>
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
