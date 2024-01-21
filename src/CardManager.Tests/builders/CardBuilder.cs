using CardManager.Domain.contracts;

namespace CardManager.Tests.builders;

public class CardBuilder
{
    private readonly CardTestSubclass _card;

    public CardBuilder()
    {
        _card = BuildDefault();
    }

    public Card Build()
    {
        return _card;
    }

    public CardBuilder WithUserId(string userId)
    {
        _card.UpdateUserId(userId);

        return this;
    }

    public CardBuilder Physical()
    {
        _card.UpdateType(CardType.Physical);

        return this;
    }

    public CardBuilder Virtual()
    {
        _card.UpdateType(CardType.Virtual);

        return this;
    }

    public CardBuilder Cancelled()
    {
        _card.UpdateStatus(CardStatus.Cancelled);

        return this;
    }

    public CardBuilder Expired(DateTime currentDate)
    {
        _card.UpdateCreatedAt(currentDate.Subtract(new TimeSpan(0, 0, 2)));
        _card.UpdateExpiresAt(currentDate.Subtract(new TimeSpan(0, 0, 1)));

        return this;
    }

    public static IList<Card> BuildNonActivePhysicalCards(string userId, DateTime currentDate)
    {
        var expiredCard = new CardBuilder()
            .WithUserId(userId)
            .Physical()
            .Expired(currentDate)
            .Build();

        var cancelledCard = new CardBuilder()
            .WithUserId(userId)
            .Physical()
            .Cancelled()
            .Build();

        return new List<Card> { cancelledCard, expiredCard };
    }

    private static CardTestSubclass BuildDefault()
    {
        return new CardTestSubclass(
            "117356",
            "58235",
            CardStatus.Unblocked,
            CardType.Physical,
            "5555111122223333",
            687,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1)
        );
    }
}
