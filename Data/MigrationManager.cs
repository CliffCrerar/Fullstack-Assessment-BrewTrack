using BrewTrack.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BrewTrack.DataContext
{

    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            using (var loggerFac = new LoggerFactory())
            {
                var logger = loggerFac.CreateLogger("Migrations Manager");
                using (var scope = webApp.Services.CreateScope())
                {
                    using (BrewTrackDbContext appContext = scope.ServiceProvider.GetRequiredService<BrewTrackDbContext>())
                    {
                        try
                        {
                            logger.LogInformation("--> Running Migrations <--");
                            appContext.Database.Migrate();
                            var modelBuilder = new ModelBuilder();
                        }
                        catch (Exception ex)
                        {
                            //Log errors or do anything you think it's needed
                            logger.LogError(">--> !!!!! MIGRATIONS ERROR!!!!! <--<");
                            Console.Error.WriteLine(ex);
                            throw;
                        }
                    }
                }
                return webApp;
            }
        }
    }
}
