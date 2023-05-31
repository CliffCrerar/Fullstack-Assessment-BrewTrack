using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Models
{
    [Table("api_sources")]
    public class ApiSource
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("ApiSourceId")]
        public Guid Id { get; set; }
        [Required]
        public string ApiSourceName { get; set; }
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        [ForeignKey("ApiSourceRefId")]
        public CachedTimeline[] Timeline { get; set; }
    }
}
