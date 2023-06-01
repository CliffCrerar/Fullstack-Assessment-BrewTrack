using BrewTrack.Contracts.IUser;
using System.ComponentModel.DataAnnotations;

namespace BrewTrack.Dto
{
    public class UserCreateRequestDto : IUserCreateRequestDto
    {
        [Required(ErrorMessage = "Email Address is Required"), EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;
        [Required(ErrorMessage = "Name is Required")]
        public string GivenName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Surname is Required")]
        public string FamilyName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Date of Birth is Required")]
        public DateTime DateOfBirth { get; set; }
    }
}
