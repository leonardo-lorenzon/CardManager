using CardManager.Domain.contracts;

namespace CardManager.Tests.builders;

public class UserBuilder
{
    private readonly UserTestSubclass _user;

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
        _user.UpdateStatus(UserStatus.Active);

        return this;
    }

    public UserBuilder Blocked()
    {
        _user.UpdateStatus(UserStatus.Blocked);

        return this;
    }

    private static UserTestSubclass BuildDefault()
    {
        return new UserTestSubclass("7610", "Astrid", UserStatus.Active);
    }
}
