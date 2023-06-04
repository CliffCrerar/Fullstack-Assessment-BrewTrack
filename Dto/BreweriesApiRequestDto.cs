using BrewTrack.Contracts.IBrewery;

namespace BrewTrack.Dto
{
    public class BreweriesApiRequestDto : IBrewPub
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Website_Url { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Brewery_Type { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
    }
}
