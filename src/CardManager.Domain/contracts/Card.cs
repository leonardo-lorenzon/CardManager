namespace CardManager.Domain.contracts;

public class Card
{
    public string Id { get; }
    public string UserId { get; }
    public CardStatus Status { get; }
    public CardType Type { get; }
    public string Number { get; }
    public int Cvv { get; }
    public DateTime CreatedAt { get; }
    public DateTime ExpiresAt { get; }

    public Card(
        string id,
        string userId,
        CardStatus status,
        CardType type,
        string number,
        int cvv,
        DateTime createdAt,
        DateTime expiresAt
        )
    {
        Id = id;
        UserId = userId;
        Status = status;
        Type = type;
        Number = number;
        Cvv = cvv;
        ExpiresAt = expiresAt;
        CreatedAt = createdAt;
    }
}
