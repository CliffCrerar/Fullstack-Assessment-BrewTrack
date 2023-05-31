using BrewTrack.Contracts;
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




        public async Task<IBrewPubApi[]> GetData()
        {
            
            var data = await AppHttpClient.GetAsync(_apiUrl);
            var jsonData = JsonSerializer.Deserialize<BrewPub[]>(data) ?? new [] { new BrewPub() };
            Ensure.ArgumentNotNull(jsonData, nameof(jsonData));
            if(jsonData != null)
            {
                throw new NullReferenceException("Data returned from ");
            }
            return jsonData;
        }
    }
}
