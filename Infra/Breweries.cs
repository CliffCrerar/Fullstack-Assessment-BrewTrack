using BrewTrack.Contracts;
using BrewTrack.Helpers;
using Google.Protobuf.WellKnownTypes;
using System.Security.Policy;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;
using MySqlX.XDevAPI.Relational;
using BrewTrack.Models;

namespace BrewTrack.Infra
{
    public class Breweries
    {
        private string _apiUrl = "https://api.openbrewerydb.org/v1/breweries";
        public Breweries()
        {
        }


        public async Task<object> GetData()
        {
            var data = await AppHttpClient.GetAsync(_apiUrl);
            var jsonData = JsonSerializer.Deserialize<BrewPubs>(data);
            return Task.FromResult<IBrewPubApi>(jsonData);
        }
    }
}
