using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Models
{
    [Table("users")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key, Column("UserId")]
        public int Id { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
