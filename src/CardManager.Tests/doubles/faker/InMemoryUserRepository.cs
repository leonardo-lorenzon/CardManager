using CardManager.Domain.contracts;
using CardManager.Domain.exceptions;
using CardManager.Domain.repositories;

namespace CardManager.Tests.doubles.faker;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public User Fetch(string userId)
    {
        var user = _users.Find(item => item.UserId == userId);

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        return user;
    }

    // only for test purposes
    public void AddUser(User user)
    {
        _users.Add(user);
    }
}
