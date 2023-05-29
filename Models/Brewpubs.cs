using BrewTrack.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Models
{
    [Table("brewpubs")]
    public class BrewPubs: IBrewPubApi
    {
        [Key, Column("brewPubId")]
        public string Id { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public string Website_Uri { get; set; }
        public string Phone { get; set; }
    }
}
