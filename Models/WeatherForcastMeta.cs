using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Models
{
    [Table("weather_forcast_header"), PrimaryKey(nameof(Id)), Index("Lat", "Lng", IsUnique = true)]
    public class WeatherForcastMeta
    {
        [Key, Column("WeatherForecastHeaderId")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [ForeignKey("CachedTimeLineId")] public Guid CachedTimeLineId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string Lat { get; set; }
        public string Lng { get; set; }
        public int Cost { get; set; }
        public int DailyQuota { get; set; }
        public string End { get; set; }
        [NotMapped]
        public string[] Params { get; set; }
        public int RequestCount { get; set; }
        [NotMapped]
        public string[] Source { get; set; }
        public string Start { get; set; }
    }
}

