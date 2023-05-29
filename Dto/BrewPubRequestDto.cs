using BrewTrack.Contracts;

namespace BrewTrack.Dto
{
    public class BrewPubRequestDto : IBrewPub
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public string Website_Uri { get; set; }
        public string Phone { get; set; }
    }
}
