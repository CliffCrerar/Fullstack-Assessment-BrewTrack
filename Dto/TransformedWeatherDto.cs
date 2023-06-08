using System;
using BrewTrack.Models;

namespace BrewTrack.Dto
{
	public class TransformedWeatherDto
	{
        public IList<WeatherForecastDay> TemperaturesPerDays { get; set; } = new List<WeatherForecastDay>();
        public WeatherForecastPageMeta? Meta { get; set; }
    }

    public class WeatherForecastHttpResponse: TransformedWeatherDto
    {
        public new WeatherForcastMeta Meta { get; set; }
    }

}

