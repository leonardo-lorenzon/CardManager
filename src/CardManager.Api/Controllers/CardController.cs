using CardManager.Api.Controllers.responses;
using CardManager.Domain.contracts;
using CardManager.Domain.services;
using Microsoft.AspNetCore.Mvc;

namespace CardManager.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CardController : ControllerBase
{
    private readonly ICardService _cardService;

    public CardController(ICardService cardService)
    {
        _cardService = cardService;
    }

    [HttpGet("{userId}")]
    public IEnumerable<CardResponse> ListCards(string userId)
    {
        var cards = _cardService.ListByUserId(userId);

        return cards.Select(item => new CardResponse(item)).ToArray();
    }

    [HttpPost]
    [Route("{cardType}/{userId}")]
    public ActionResult<CardResponse> CreateCard(string cardType, string userId)
    {
        var card = _cardService.Create(userId, Enum.Parse<CardType>(cardType));

        return new CardResponse(card);
    }
}
