using BrewTrack.Dto;
using BrewTrack.Infra;
using BrewTrack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrewTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWeatherService _weatherService;
        public WeatherController(IConfiguration config, IWeatherService weatherService) 
        {
            _configuration = config;
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string longitude, [FromQuery] string latitude)
        {
            TransformedWeatherDto weatherForecast = await _weatherService.GetWeatherForecast(latitude, longitude);
            return Ok(weatherForecast);
        }
    }
}
