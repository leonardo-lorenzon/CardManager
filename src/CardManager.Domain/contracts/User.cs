namespace CardManager.Domain.contracts;

public class User
{
    public string UserId { get; }

    public string Name { get; }

    public UserStatus Status { get; }

    public User(string userId, string name, UserStatus status)
    {
        UserId = userId;
        Name = name;
        Status = status;
    }
}
