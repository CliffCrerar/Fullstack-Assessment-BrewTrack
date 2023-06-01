using BrewTrack.Contracts;
using BrewTrack.Contracts.IBrewery;
using BrewTrack.Helpers;
using BrewTrack.Models;
using System.Text.Json;

namespace BrewTrack.Infra
{
    public class Breweries
    {
        private string _apiUrl = "https://api.openbrewerydb.org/v1/breweries";
        public Breweries()
        {
        }

        public async Task<IBrewPub[]> GetData()
        {
            
            var data = await AppHttpClient.GetAsync(_apiUrl);
            var jsonData = JsonSerializer.Deserialize<BrewPub[]>(data) 
                ?? throw new NullReferenceException("Data returned from ");
            return jsonData;
        }
    }
}
