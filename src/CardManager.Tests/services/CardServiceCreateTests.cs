using CardManager.Domain.contracts;
using CardManager.Domain.exceptions;
using CardManager.Domain.services;
using CardManager.Tests.doubles.faker;
using CardManager.Tests.factories;
using Xunit;

namespace CardManager.Tests.services;

public class CardServiceCreateTests
{
    [Fact]
    public void ShouldCreatePhysicalCardWithStatusIssued()
    {
        // Arrange
        var (cardService, cardRepository, user) = SetupSutWithActiveUser();

        // Act
        cardService.Create(user.UserId, CardType.Physical);

        // Assert
        var result = cardRepository.ListByUserId(user.UserId);

        Assert.Equal(1, result.Count);
        Assert.Equal(CardStatus.Issued, result[0].Status);
    }

    [Fact]
    public void ShouldCreateVirtualCardWithStatusUnblocked()
    {
        // Arrange
        var (cardService, cardRepository, user) = SetupSutWithActiveUser();

        // Act
        cardService.Create(user.UserId, CardType.Virtual);

        // Assert
        var result = cardRepository.ListByUserId(user.UserId);

        Assert.Equal(1, result.Count);
        Assert.Equal(CardStatus.Unblocked, result[0].Status);
    }

    [Fact]
    public void ShouldBeAbleToCreatePhysicalCardIfThereIsNoActivePhysicalCard()
    {
        // Arrange
        var user = new User("123", "Astrid", UserStatus.Active);

        var card1 = new Card(
            "111",
            user.UserId,
            CardStatus.Cancelled,
            CardType.Physical,
            "5555111122223333",
            123,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        var card2 = new Card(
            "222",
            user.UserId,
            CardStatus.Unblocked,
            CardType.Physical,
            "5555111122223333",
            123,
            DateTime.UtcNow,
            DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0))
        );

        var factory = new CardServiceTestFactory();
        factory.AddCards(new List<Card> { card1, card2 });
        factory.SetUser(user);

        var cardRepository = factory.CardRepository;
        var cardService = factory.Build();

        // Act
        cardService.Create(user.UserId, CardType.Physical);

        // Assert
        var result = cardRepository.ListByUserId(user.UserId);

        Assert.Equal(3, result.Count);
        Assert.Equal(CardStatus.Issued, result[2].Status);
    }

    [Fact]
    public void ShouldThrowExceptionWhenUserIsBlocked()
    {
        // Arrange
        var (cardService, user) = SetupSutWithBlockedUser();

        // Act

        // Assert
        Assert.Throws<CannotPhysicalCardException>(() => cardService.Create(user.UserId, CardType.Physical));
    }

    [Fact]
    public void ShouldThrowExceptionWhenUserIsNotFound()
    {
        // Arrange
        var userId = "123";

        var cardService = new CardServiceTestFactory().Build();

        // Act

        // Assert
        Assert.Throws<CannotPhysicalCardException>(() => cardService.Create(userId, CardType.Physical));
    }

    [Fact]
    public void ShouldThrowExceptionWhenTryToCreateMoreThanOneActivePhysicalCard()
    {
        // Arrange
        var user = new User("123", "Astrid", UserStatus.Active);

        var card = new Card(
            "111",
            user.UserId,
            CardStatus.Issued,
            CardType.Physical,
            "5555111122223333",
            123,
            DateTime.UtcNow,
            DateTime.UtcNow.AddYears(1)
        );

        var factory = new CardServiceTestFactory();
        factory.SetUser(user);
        factory.AddCards(new List<Card> { card });

        var cardService = factory.Build();

        // Act
        // Assert
        Assert.Throws<CannotPhysicalCardException>(() => cardService.Create(user.UserId, CardType.Physical));
    }

    [Fact]
    public void ShouldBeAbleToCreateMoreThanOneVirtualCard()
    {
        // Arrange
        var user = new User("123", "Astrid", UserStatus.Active);

        var card = new Card(
            "111",
            user.UserId,
            CardStatus.Issued,
            CardType.Virtual,
            "5555111122223333",
            123,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        var factory = new CardServiceTestFactory();
        factory.SetUser(user);
        factory.AddCards(new List<Card> { card });

        var cardRepository = factory.CardRepository;
        var cardService = factory.Build();

        // Act
        cardService.Create(user.UserId, CardType.Virtual);


        // Assert
        var result = cardRepository.ListByUserId(user.UserId);
        Assert.Equal(2, result.Count);
    }

    private static (CardService cardService, InMemoryCardRepository cardRepository, User user) SetupSutWithActiveUser()
    {
        var user = new User("123", "Astrid", UserStatus.Active);

        var factory = new CardServiceTestFactory();
        factory.SetUser(user);

        var cardRepository = factory.CardRepository;
        var cardService = factory.Build();

        return (cardService, cardRepository, user);
    }

    private static (CardService cardService, User user) SetupSutWithBlockedUser()
    {
        var user = new User("123", "Astrid", UserStatus.Blocked);

        var factory = new CardServiceTestFactory();
        factory.SetUser(user);
        var cardService = factory.Build();

        return (cardService, user);
    }
}
