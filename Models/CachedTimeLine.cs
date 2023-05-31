using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Models
{
    [Table("cached_timeline"), Index(nameof(Date), IsDescending = new [] { true })]
    public class CachedTimeline
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime Date {get;set;}
        [Required]
        public Guid ApiSourceRefId { get; set;}
        
        
    }
}
