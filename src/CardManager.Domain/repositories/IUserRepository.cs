using CardManager.Domain.contracts;

namespace CardManager.Domain.repositories;

public interface IUserRepository
{
    public User Fetch(string userId);
}
