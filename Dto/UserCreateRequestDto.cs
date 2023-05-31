using BrewTrack.Contracts.IUser;
using System.ComponentModel.DataAnnotations;

namespace BrewTrack.Dto
{
    public class UserCreateRequestDto : IUserCreateRequestDto
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
