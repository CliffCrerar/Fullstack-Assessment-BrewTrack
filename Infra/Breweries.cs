using BrewTrack.Contracts.IBrewery;
using BrewTrack.Dto;
using BrewTrack.Helpers;
using BrewTrack.Models;

namespace BrewTrack.Infra
{
    /// <summary>
    /// Breweries api integration infrastructure;
    /// </summary>
    public class Breweries
    {
        /// <summary>
        /// Api Url
        /// </summary>
        private static string _apiUrl = BrewTrackContstants.BreweryApiResource;
        /// <summary>
        /// Logger factory member
        /// </summary>
        private static LoggerFactory _LoggerFac;
        /// <summary>
        /// Create logger factory
        /// </summary>
        /// <returns>ILogger<Breweries></returns>
        private static ILogger<Breweries> _logger()
        {
            _LoggerFac = new LoggerFactory();
            return _LoggerFac.CreateLogger<Breweries>();
        }
        /// <summary>
        /// Dispose of logger factory
        /// </summary>
        private static void _disposeLoggerFac() { _LoggerFac.Dispose(); }
        /// <summary>
        /// Get the data from the breweries api
        /// </summary>
        /// <returns>Task<List<BrewPub>></returns>
        public static async Task<IList<BrewPub>> GetData()
        {
            using (HttpClient client = new HttpClient())
            {
                var logger = _logger();
                logger.LogInformation("Getting Data from Api");
                try
                {
                    var response = await client.GetFromJsonAsync<List<BrewPub>>(_apiUrl);

                    IList<BrewPub> converted = _mapApiData(Ensure.ArgumentNotNull(response));
                    _disposeLoggerFac();
                    logger.LogInformation("Data Retrieved");
                    return converted;
                } 
                catch (Exception ex)
                {
                    logger.LogInformation("Error Retreiving data from Breweries API");
                    logger.LogError(ex.Message, ex);
                    throw;
                }

            }
        }

        private static List<BrewPub> _mapApiData(List<BrewPub> data)
        {
            
            return data;
        }
    }
}
