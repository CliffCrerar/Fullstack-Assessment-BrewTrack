using BrewTrack.Contracts.IBrewery;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Dto
{
    public class BreweriesApiRequestDto : IBrewPub
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Website_Uri { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
    }
}
