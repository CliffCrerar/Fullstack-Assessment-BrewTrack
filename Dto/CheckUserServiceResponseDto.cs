using BrewTrack.Contracts;
using BrewTrack.Contracts.IUser;
using BrewTrack.Models;
using System.ComponentModel.DataAnnotations;

namespace BrewTrack.Dto
{
    public class CreateUserRequest : IUserCreateRequestDto
    {
        [Required, EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;
        [Required]
        public string GivenName { get; set; } = string.Empty;
        [Required]
        public string FamilyName { get; set; } = string.Empty;
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
