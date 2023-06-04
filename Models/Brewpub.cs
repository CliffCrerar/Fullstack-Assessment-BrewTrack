using BrewTrack.Contracts.IBrewery;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BrewTrack.Models
{
    [Table("brewpubs")]
    public class BrewPub: IBrewPub
    {
        [Key, Column("brewPubId")]
        public Guid Id { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public string Name { get; set; }
        public string? City { get; set; }
        public string? Brewery_Type { get; set; }
        public string? Website_Url { get; set; }
        // [Required(ErrorMessage = "Required")]
        // [Phone ]
        // [RegularExpression(@"^(\+\d{1, 2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$", ErrorMessage = "Not a valid phone number")]
        public string? Phone { get; set; }
    }
}
