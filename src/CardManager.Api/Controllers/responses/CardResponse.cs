using System.Globalization;
using CardManager.Domain.contracts;

namespace CardManager.Api.Controllers.responses;

public class CardResponse
{
    public string Id { get; }
    public string UserId { get; }
    public string Status { get; }
    public string Type { get; }
    public string Number { get; }
    public int Cvv { get; }
    public string CreatedAt { get; }
    public string ExpiresAt { get; }

    public CardResponse(Card card)
    {
        Id = card.Id;
        UserId = card.UserId;
        Status = card.Status.ToString();
        Type = card.Type.ToString();
        Number = card.Number;
        Cvv = card.Cvv;
        CreatedAt = card.CreatedAt.ToString(CultureInfo.InvariantCulture);
        ExpiresAt = card.ExpiresAt.ToString(CultureInfo.InvariantCulture);
    }


}
