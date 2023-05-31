namespace BrewTrack.Contracts.IUser
{
    public interface IUserCreateRequestDto
    {
        string EmailAddress { get; set; }
        string GivenName { get; set; }
        string FamilyName { get; set; }
        DateTime DateOfBirth { get; set; }
    }

    public interface IUser : IUserCreateRequestDto
    {
        Guid Id { get; set; }
    }
}
