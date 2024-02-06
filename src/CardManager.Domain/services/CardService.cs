using System.Globalization;
using System.Security.Cryptography;
using CardManager.Domain.contracts;
using CardManager.Domain.exceptions;
using CardManager.Domain.repositories;

namespace CardManager.Domain.services;

public class CardService : ICardService
{
    public const int PhysicalCardExpirationYears = 3;
    public const int VirtualCardExpirationYears = 2;

    private readonly ICardRepository _cardRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITimeService _timeService;

    public CardService(ICardRepository cardRepository, IUserRepository userRepository, ITimeService timeService)
    {
        _cardRepository = cardRepository;
        _userRepository = userRepository;
        _timeService = timeService;
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
        try
        {
            var user = _userRepository.Fetch(userId);

            if (user.Status == UserStatus.Blocked)
            {
                throw new CannotPhysicalCardException("Cannot create card. User blocked");
            }

            if (cardType == CardType.Physical)
            {
                return CreatePhysicalCard(userId);
            }

            return CreateVirtualCard(userId);
        }
        catch (UserNotFoundException)
        {
            throw new CannotPhysicalCardException("Cannot create card. User not found");
        }
    }

    private Card CreatePhysicalCard(string userId)
    {
        var cards = _cardRepository.ListByUserId(userId);

        var physicalCards = cards.Where(
            item => item.IsPhysical() && item.IsActive(_timeService.CurrentDate())
            ).ToList();

        if (physicalCards.Count > 0)
        {
            throw new CannotPhysicalCardException("Cannot have more than one active physical card");
        }

        var card = new Card(
            GenerateCardId(),
            userId,
            CardStatus.Issued,
            CardType.Physical,
            GenerateCardNumber(),
            GenerateCvv(),
            _timeService.CurrentDate(),
            CreateExpiredDate(CardType.Physical)
        );

        _cardRepository.Create(card);

        return card;
    }

    private Card CreateVirtualCard(string userId)
    {
        var card = new Card(
            GenerateCardId(),
            userId,
            CardStatus.Unblocked,
            CardType.Virtual,
            GenerateCardNumber(),
            GenerateCvv(),
            _timeService.CurrentDate(),
            CreateExpiredDate(CardType.Virtual)
        );

        _cardRepository.Create(card);

        return card;
    }

    private DateTime CreateExpiredDate(CardType cardType)
    {
        if (cardType == CardType.Physical)
        {
            return _timeService.CurrentDate().AddYears(PhysicalCardExpirationYears);
        }

        return _timeService.CurrentDate().AddYears(VirtualCardExpirationYears);
    }

    private static string GenerateCardId()
    {
        return RandomNumberGenerator.GetInt32(1000000000).ToString(CultureInfo.InvariantCulture);
    }

    private static int GenerateCvv()
    {
        return RandomNumberGenerator.GetInt32(111, 999);
    }

    private static string GenerateCardNumber()
    {
        var randomNumber = RandomNumberGenerator.GetInt32(1111, 9999).ToString(CultureInfo.InvariantCulture);

        return $"555511112222{randomNumber}";
    }
}
