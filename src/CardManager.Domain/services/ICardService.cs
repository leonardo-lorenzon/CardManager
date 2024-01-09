using CardManager.Domain.contracts;

namespace CardManager.Domain.services;

public interface ICardService
{
    public IList<Card> ListByUserId(string userId);

    public Card Create(string userId, CardType cardType);
}
