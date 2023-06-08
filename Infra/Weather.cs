using BrewTrack.Dto;
using BrewTrack.Helpers;
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

                // temp code for testing
                //var tempUrl = "https://gist.githubusercontent.com/CliffCrerar/569b66b170ddf9fbce57a1908445c82e/raw/8f276a10c1757f2fec0a1ec110248c37cb0c7d98/weather";
                //var responseBody =  Ensure.ArgumentNotNull(await client.GetFromJsonAsync<WeatherForeCastApiRequestDto>(tempUrl));
                // temp code for testing

                return responseBody;
            }
        }
    }
}
