using CardManager.Domain.contracts;

namespace CardManager.Tests.builders;

public class UserTestSubclass : User
{
    public UserTestSubclass(string userId, string name, UserStatus status) : base(userId, name, status)
    {
    }

    public void UpdateStatus(UserStatus userStatus)
    {
        Status = userStatus;
    }
}
