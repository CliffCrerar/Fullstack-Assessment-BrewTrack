using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Models
{
    [Table("cached_timeline"), Index(nameof(Date), IsDescending = new [] { true })]
    public class CachedTimeline
    {
        [Key, Column("CachedTimeLineId")]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Date {get;set;}
        [Required]
        public Guid ApiSourceRefId { get; set;}       
        
    }
}
