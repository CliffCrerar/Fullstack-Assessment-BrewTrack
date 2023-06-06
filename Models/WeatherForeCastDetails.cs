using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Models
{
    [Table("weather_forecast_details"), PrimaryKey("Id")]
    public class WeatherForeCastDetails
    {
        [Key, Column("WeatherForeCastDetailsId")]
        public Guid Id { get; set; } = Guid.NewGuid();
        public int AirTemprature { get; set; }
        public DateTime Time { get; set; }
        [ForeignKey("WeatherForCastHeaderId")] public Guid WeatherForCastHeaderId { get; set; }
    }
}
