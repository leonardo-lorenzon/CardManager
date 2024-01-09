using CardManager.Domain.contracts;

namespace CardManager.Domain.repositories;

public interface ICardRepository
{
    public IList<Card> ListByUserId(string userId);

    public void Create(Card card);
}
