using CardManager.Domain.contracts;

namespace CardManager.Tests.builders;

public class CardTestSubclass : Card
{
    public CardTestSubclass(
        string id,
        string userId,
        CardStatus status,
        CardType type,
        string number,
        int cvv,
        DateTime createdAt,
        DateTime expiresAt)
        : base(id, userId, status, type, number, cvv, createdAt, expiresAt)
    {
    }

    public void UpdateUserId(string userId)
    {
        UserId = userId;
    }

    public void UpdateType(CardType cardType)
    {
        Type = cardType;
    }

    public void UpdateStatus(CardStatus cardStatus)
    {
        Status = cardStatus;
    }

    public void UpdateCreatedAt(DateTime createdAt)
    {
        CreatedAt = createdAt;
    }

    public void UpdateExpiresAt(DateTime expiresAt)
    {
        ExpiresAt = expiresAt;
    }
}
