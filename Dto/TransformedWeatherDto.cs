using System;
namespace BrewTrack.Dto
{
	public class TransformedWeatherDto
	{
        public IList<TemperaturesPerDay> TemperaturesPerDays { get; set; } = new List<TemperaturesPerDay>();
        public WeatherForecastMeta Meta { get; set; }
    }

	public class TemperaturesPerDay
	{
        public int Day { get; set; }
        public DateTime FullDate { get; set; }
        public decimal AverageTemperature { get; set; }
        public IList<TemperaturePerHour> Temperatures { get; set; } = new List<TemperaturePerHour>();
    }

    public class TemperaturePerHour
    {
        public int Hour { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime FullDate { get; set; }
        public decimal AirTemperature { get; set; }
    }
}

