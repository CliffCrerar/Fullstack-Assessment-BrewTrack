using BrewTrack.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Models
{
    [Table("brewpubs")]
    public class BrewPub: IBrewPub
    {
        [Key, Column("brewPubId")]
        public string Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public string Website_Uri { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
    }
}
