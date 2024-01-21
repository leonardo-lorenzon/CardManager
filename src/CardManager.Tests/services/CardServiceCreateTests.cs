using CardManager.Domain.contracts;
using CardManager.Domain.exceptions;
using CardManager.Domain.services;
using CardManager.Tests.builders;
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
    public void ShouldCreateAPhysicalCardWithThreeYearsExpirationDate()
    {
        // Arrange
        var fakeCurrentDate = DateTime.UtcNow;
        var user = new UserBuilder().Build();

        var factory = new CardServiceTestFactory();
        factory.SetUser(user);
        factory.SetCurrentDate(fakeCurrentDate);

        var cardRepository = factory.CardRepository;
        var cardService = factory.Build();

        // Act
        cardService.Create(user.UserId, CardType.Physical);

        // Assert

        var result = cardRepository.ListByUserId(user.UserId);

        var expectedExpiredAt = fakeCurrentDate.AddYears(3);
        Assert.Equal(expectedExpiredAt, result[0].ExpiresAt);
    }


    [Fact]
    public void ShouldCreateAVirtualCardWithTwoYearsExpirationDate()
    {
        // Arrange
        var fakeCurrentDate = DateTime.UtcNow;
        var user = new UserBuilder().Build();

        var factory = new CardServiceTestFactory();
        factory.SetUser(user);
        factory.SetCurrentDate(fakeCurrentDate);

        var cardRepository = factory.CardRepository;
        var cardService = factory.Build();

        // Act
        cardService.Create(user.UserId, CardType.Virtual);

        // Assert

        var result = cardRepository.ListByUserId(user.UserId);

        var expectedExpiredAt = fakeCurrentDate.AddYears(2);
        Assert.Equal(expectedExpiredAt, result[0].ExpiresAt);
    }

    [Fact]
    public void ShouldBeAbleToCreatePhysicalCardIfThereIsNoActivePhysicalCard()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var fakerCurrentDate = DateTime.UtcNow;

        var nonActivePhysicalCards = CardBuilder.BuildNonActivePhysicalCards(user.UserId, fakerCurrentDate);

        var factory = new CardServiceTestFactory();
        factory.AddCards(nonActivePhysicalCards);
        factory.SetUser(user);
        factory.SetCurrentDate(fakerCurrentDate);

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
        var user = new UserBuilder().Build();

        var card = new CardBuilder()
            .WithUserId(user.UserId)
            .Physical()
            .Build();

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
        var user = new UserBuilder().Build();

        var card = new CardBuilder()
            .WithUserId(user.UserId)
            .Virtual()
            .Build();

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
        Assert.Equal(CardStatus.Unblocked, result[0].Status);
    }

    private static (CardService cardService, InMemoryCardRepository cardRepository, User user) SetupSutWithActiveUser()
    {
        var user = new UserBuilder().Build();

        var factory = new CardServiceTestFactory();
        factory.SetUser(user);

        var cardRepository = factory.CardRepository;
        var cardService = factory.Build();

        return (cardService, cardRepository, user);
    }

    private static (CardService cardService, User user) SetupSutWithBlockedUser()
    {
        var user = new UserBuilder()
            .Blocked()
            .Build();

        var factory = new CardServiceTestFactory();
        factory.SetUser(user);
        var cardService = factory.Build();

        return (cardService, user);
    }
}
