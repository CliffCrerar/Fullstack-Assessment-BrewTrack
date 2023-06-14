using BrewTrack.Data;
using BrewTrack.Helpers;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace BrewTrack.DataContext
{

    public static class MigrationManager
    {
        private static string? _connectionString;
        private static bool _testDatabaseConnection()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    bool canConnect;
                    connection.Open();
                    canConnect = connection.Ping();
                    connection.CloseAsync();
                    return canConnect;
                } catch(Exception ex)
                {
                    Console.WriteLine("ERROR!! Cannot connect to database");
                    Console.Error.WriteLine(ex.Message);
                    return false;
                }

            }
        }
        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            _connectionString = Ensure.ArgumentNotNull(webApp.Configuration.GetConnectionString("MySql"));
            using (var loggerFac = new LoggerFactory())
            {
                var logger = loggerFac.CreateLogger("Migrations Manager");
                using (var scope = webApp.Services.CreateScope())
                {
                    using (BrewTrackDbContext appContext = scope.ServiceProvider.GetRequiredService<BrewTrackDbContext>())
                    {
                        try
                        {
                            if (_testDatabaseConnection())
                            {
                                logger.LogInformation("--> Running Migrations <--");
                                appContext.Database.Migrate();
                            }
                            else
                            {
                                throw new Exception("Database is not running");
                            }
                        }
                        catch (Exception ex)
                        {
                            //Log errors or do anything you think it's needed
                            logger.LogError(">--> !!!!! MIGRATIONS ERROR!!!!! <--<");
                            Console.Error.WriteLine(ex.Message);
                            // throw;
                        }
                    }
                }
                return webApp;
            }
        }
    }
}
