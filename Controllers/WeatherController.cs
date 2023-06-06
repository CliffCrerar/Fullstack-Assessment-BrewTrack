using BrewTrack.Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrewTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public WeatherController(IConfiguration config) 
        {
            _configuration = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string longitude, [FromQuery] string latitude)
        {
            string config = _configuration.GetValue<string>("WeatherApiKey");
            var weatherApi = new Weather(config);
            return Ok(await weatherApi.GetWeatherForLocation(latitude, longitude));
        }
    }
}
