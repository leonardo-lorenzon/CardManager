using CardManager.Domain.contracts;
using CardManager.Domain.exceptions;
using CardManager.Domain.repositories;

namespace CardManager.Infrastructure.repositories;

public class CardRepository : ICardRepository
{
    private readonly List<Card> _cards = new();

    public IList<Card> ListByUserId(string userId)
    {
        var card = _cards.FindAll(item => item.UserId == userId);

        if (card is null)
        {
            throw new CardNotFoundException();
        }

        return card;
    }

    public void Create(Card card)
    {
        _cards.Add(card);
    }
}
