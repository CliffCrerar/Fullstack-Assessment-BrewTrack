using BrewTrack.Dto;
using System.Data;
using System.Net;
using System.Net.Http.Headers;

namespace BrewTrack.Infra
{
    public class Weather
    {
        private string _apiKey;
        public Weather(string ApiKey)
        {
            _apiKey = ApiKey;
        }

        public async Task<WeatherForeCastApiRequestDto> GetWeatherForLocation(string latitude, string longitude)
        {
            string apiResponseParams = "airTemperature";
            string requestUri = string.Format(
                "{0}?lat={1}&lng={2}&params={3}&source=noaa", 
                BrewTrackContstants.WeatherApiResource,
                latitude,
                longitude,
                apiResponseParams
                );
            HttpRequestMessage req = new HttpRequestMessage();
            req.RequestUri = new Uri(requestUri);
            req.Method = HttpMethod.Get;
            req.Headers.Authorization = new AuthenticationHeaderValue(_apiKey);
            
            using(var client = new HttpClient())
            {

                HttpResponseMessage response = await client.SendAsync(req);
                var responseBody = await response.Content.ReadFromJsonAsync<WeatherForeCastApiRequestDto>();
                return responseBody;
            }
        }
    }
}
