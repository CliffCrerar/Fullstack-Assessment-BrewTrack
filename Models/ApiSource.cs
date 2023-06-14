using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Models
{
    [Table("api_sources")]
    public class ApiSource
    {
        [Key, Column("ApiSourceId")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string ApiSourceName { get; set; } = string.Empty;
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        [ForeignKey("ApiSourceRefId")]
        public CachedTimeline[] Timeline { get; set; } = new CachedTimeline[] {  };
    }
}
