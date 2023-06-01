using BrewTrack.Contracts.IUser;
using BrewTrack.Data;
using BrewTrack.Dto;
using BrewTrack.Helpers;
using BrewTrack.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BrewTrack.Services;
public static class UserServiceExtension
{
    public static IServiceCollection AddUserService(this IServiceCollection services)
    {
        return services.AddScoped<IUserService, UserService>(provider =>
        {
            var dbContext = Ensure.ArgumentNotNull(provider.GetService<BrewTrackDbContext>());
            return new UserService(dbContext);
        });
    }
}

public interface IUserService
{
    public bool CheckUserByEmail(string email);
    public bool CheckUserById(Guid userId);
    public Task<User> CreateUser(IUserCreateRequestDto userRequest);
    public User? GetUserByEmail(string email);
    public User? GetUserById(Guid userId);
    public User? User { get; }
}

public class UserService : IUserService
{
    private readonly BrewTrackDbContext _dbContext;
    private User? _user;
    public User? User { get => _user; }

    public UserService(BrewTrackDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool CheckUserByEmail(string email)
    {
        var user = GetUserByEmail(email);
        _user = user;
        return user != null;
    }

    public bool CheckUserById(Guid userId)
    {
        var user = GetUserById(userId);
        _user = user;
        return user != null;
    }

    public async Task<User> CreateUser(IUserCreateRequestDto userRequest)
    {
        try
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                GivenName = userRequest.GivenName,
                EmailAddress = userRequest.EmailAddress,
                DateOfBirth = userRequest.DateOfBirth,
                FamilyName = userRequest.FamilyName
            };
            EntityEntry<User> createdUserEntity = _dbContext.Add<User>(user);
            await _dbContext.SaveChangesAsync();
            await createdUserEntity.ReloadAsync();
            _user = createdUserEntity.Entity;
            return _user;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            throw;
        }
    }

    public User? GetUserById(Guid userId)
    {
        if(_user != null && _user.Id == userId) return _user;
        _user = _dbContext.Users.Where(row => row.Id == userId).FirstOrDefault();
        return _user;
    }

    public User? GetUserByEmail(string email)
    {
        if (_user != null && _user.EmailAddress == email) return _user;
        _user = _dbContext.Users.Where(row => row.EmailAddress == email).FirstOrDefault();
        return _user;
    }

}
