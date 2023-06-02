using BrewTrack.Contracts;
using BrewTrack.Contracts.IBrewery;
using BrewTrack.Dto;
using BrewTrack.Helpers;
using BrewTrack.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Net.WebRequestMethods;

namespace BrewTrack.Infra
{
    
    public class Breweries
    {
        private static string _apiUrl = "https://api.openbrewerydb.org/v1/breweries";
        
        public static async Task<List<BrewPub>> GetData()
        {
            using (HttpClient client = new HttpClient())
            {

                var response = await client.GetFromJsonAsync<List<BrewPub>>(_apiUrl);
                // response.EnsureSuccessStatusCode();



                // dynamic jsonData = Ensure.ArgumentNotNull(JsonSerializer.Deserialize<List<BrewPub>>(data));
                return Ensure.ArgumentNotNull(response);
            }
            // var converter = new JsonConverter<BreweriesApiRequestDto>();

        }
    }
}
