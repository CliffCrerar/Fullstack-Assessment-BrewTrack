using BrewTrack.Contracts.IBrewery;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Models
{
    [Table("brewpubs")]
    public class BrewPub: IBrewPub
    {
        [Key, Column("brewPubId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "No Longitude Provided")]
        public string Longitude { get; set; } = string.Empty;
        [Required(ErrorMessage = "No Latitude Provided")]
        public string Latitude { get; set; } = string.Empty;
        [Required(ErrorMessage ="No Name Provided")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "No City Provided")]
        public string City { get; set; } = string.Empty;
        [Required(ErrorMessage = "No Type Provided")]
        public string Type { get; set; } = string.Empty;
        [Required(ErrorMessage = "No Website_Uri Provided")]
        public string Website_Uri { get; set; } = string.Empty;
        [Required(ErrorMessage = "Required")]
        // [Phone ]
        [RegularExpression(@"^(\+\d{1, 2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; } = string.Empty;
    }
}
