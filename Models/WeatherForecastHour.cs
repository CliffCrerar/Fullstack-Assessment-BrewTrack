using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BrewTrack.Models
{
    [Table("weather_forcast_hour")]
	public class WeatherForecastHour
	{
        [Key, Column("WeatherForcastHourId")]
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Hour { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime FullDate { get; set; }
        public decimal AirTemperature { get; set; }
        [ForeignKey("WeatherForCastDayId")] public Guid WeatherForeCastDayRefId { get; set; }
    }
}

