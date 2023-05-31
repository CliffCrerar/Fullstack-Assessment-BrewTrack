using BrewTrack.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BrewTrack.DataContext
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (BrewTrackDbContext appContext = scope.ServiceProvider.GetRequiredService<BrewTrackDbContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                        var modelBuilder = new ModelBuilder();
                    }
                    catch (Exception ex)
                    {
                        //Log errors or do anything you think it's needed
                        Console.Error.WriteLine(ex);
                        throw;
                    }
                }
            }
            return webApp;
        }
    }
}
