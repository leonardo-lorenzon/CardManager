using CardManager.Domain.contracts;

namespace CardManager.Tests.builders;

public class UserBuilder
{
    private readonly User _user;

    public UserBuilder()
    {
        _user = BuildDefault();
    }

    public User Build()
    {
        return _user;
    }

    public UserBuilder Active()
    {
        _user.Status = UserStatus.Active;

        return this;
    }

    public UserBuilder Blocked()
    {
        _user.Status = UserStatus.Blocked;

        return this;
    }

    private static User BuildDefault()
    {
        return new User("7610", "Astrid", UserStatus.Active);
    }
}
