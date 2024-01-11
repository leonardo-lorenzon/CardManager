using CardManager.Domain.contracts;
using CardManager.Domain.exceptions;
using CardManager.Domain.repositories;

namespace CardManager.Infrastructure.repositories;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new()
    {
        new User("123", "John", UserStatus.Active),
        new User("124", "Carl", UserStatus.Blocked)
    };

    public User Fetch(string userId)
    {
        var user = _users.Find(item => item.UserId == userId);

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        return user;
    }
}
