using BrewTrack.Contracts.IUser;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTrack.Models
{
    [Table("users"), Index( "EmailAddress", IsUnique = true )]
    public class User: IUser
    {
        [Key, Column("UserId")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}
