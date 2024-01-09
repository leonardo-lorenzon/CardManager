using System.Globalization;
using System.Security.Cryptography;
using CardManager.Domain.contracts;
using CardManager.Domain.exceptions;
using CardManager.Domain.repositories;

namespace CardManager.Domain.services;

public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;

    public CardService(ICardRepository cardRepository)
    {
        _cardRepository = cardRepository;
    }

    public IList<Card> ListByUserId(string userId)
    {
        try
        {
            return _cardRepository.ListByUserId(userId);
        }
        catch (CardNotFoundException)
        {
            return new List<Card>();
        }
    }

    public Card Create(string userId, CardType cardType)
    {
        var card = new Card(
            GenerateCardId(),
            userId,
            CardStatus.Issued,
            cardType,
            GenerateCardNumber(),
            GenerateCvv(),
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        if (cardType == CardType.Physical)
        {
            return CreatePhysicalCard(card);
        }

        return CreateVirtualCard(card);
    }

    private Card CreatePhysicalCard(Card card)
    {
        var cards = _cardRepository.ListByUserId(card.UserId);

        var physicalCards = cards.Where(item => item.Type == CardType.Physical).ToList();

        if (physicalCards.Count > 0)
        {
            throw new DuplicatedPhysicalCardException();
        }

        _cardRepository.Create(card);

        return card;
    }

    private Card CreateVirtualCard(Card card)
    {
        _cardRepository.Create(card);

        return card;
    }

    private static string GenerateCardId() =>
        RandomNumberGenerator.GetInt32(1000000000).ToString(CultureInfo.InvariantCulture);

    private static int GenerateCvv() =>
        RandomNumberGenerator.GetInt32(111, 999);

    private static string GenerateCardNumber()
    {
        var randomNumber = RandomNumberGenerator.GetInt32(1111, 9999).ToString(CultureInfo.InvariantCulture);

        return $"555511112222{randomNumber}";
    }
}
