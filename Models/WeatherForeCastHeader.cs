using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Models
{
    [Table("weather_forcast_header"),PrimaryKey(nameof(Id)), Index("Latitude","Longitude", IsUnique =true )]
    public class WeatherForeCastHeader
    {
        [Key, Column("WeatherForecastHeaderId")]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public List<WeatherForeCastDetails> WeatherForeCastDetails { get; set; } = new List<WeatherForeCastDetails>();
    }
}
