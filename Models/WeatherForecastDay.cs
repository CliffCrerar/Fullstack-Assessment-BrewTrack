using System;
using BrewTrack.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Models
{
    [Table("weather_forcast_day")]
	public class WeatherForecastDay
	{
        [Key, Column("WeatherForecastDayId")]
        public Guid Id { get; set; } = new Guid();
        public int Day { get; set; }
        public DateTime FullDate { get; set; }
        public decimal AverageTemperature { get; set; }
        public IList<WeatherForecastHour> Temperatures { get; set; } = new List<WeatherForecastHour>();
        [ForeignKey("WeatherForecastMetaId")] public Guid WeatherForcastMetaRefId { get; set; }
    }
}

