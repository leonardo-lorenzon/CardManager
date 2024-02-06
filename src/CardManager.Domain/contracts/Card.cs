namespace CardManager.Domain.contracts;

public class Card
{
    public string Id { get; }
    public string UserId { get; protected set; }
    public CardStatus Status { get; protected set; }
    public CardType Type { get; protected set; }
    public string Number { get; }
    public int Cvv { get; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime ExpiresAt { get; protected set; }

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

    public bool IsPhysical()
    {
        return Type == CardType.Physical;
    }

    public bool IsActive(DateTime currentDate)
    {
        return Status is CardStatus.Issued or CardStatus.Blocked or CardStatus.Unblocked && ExpiresAt >= currentDate;
    }
}
