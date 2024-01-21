namespace CardManager.Domain.contracts;

public class Card
{
    public string Id { get; }
    public string UserId { get; set; }
    public CardStatus Status { get; set; }
    public CardType Type { get; set; }
    public string Number { get; }
    public int Cvv { get; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }

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
