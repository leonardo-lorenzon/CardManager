using CardManager.Domain.contracts;
using CardManager.Domain.exceptions;
using CardManager.Domain.repositories;
using CardManager.Domain.services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace CardManager.Tests.services;

public class CardServiceCreateTests
{
    [Theory]
    [InlineData(CardType.Physical)]
    [InlineData(CardType.Virtual)]
    public void ShouldCreateCard(CardType cardType)
    {
        // Arrange
        var user = new User("123", "Astrid", UserStatus.Active);

        var userRepository = Substitute.For<IUserRepository>();
        userRepository.Fetch(user.UserId).Returns(user);

        var cardRepository = Substitute.For<ICardRepository>();
        var cardService = new CardService(cardRepository, userRepository);

        // Act
        cardService.Create(user.UserId, cardType);

        // Assert
        if (cardType is CardType.Physical)
        {
            cardRepository.Received(1).Create(Arg.Is<Card>(item => item.Status == CardStatus.Issued));
        }
        else
        {
            cardRepository.Received(1).Create(Arg.Is<Card>(item => item.Status == CardStatus.Unblocked));
        }
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

        var userRepository = Substitute.For<IUserRepository>();
        userRepository.Fetch(user.UserId).Returns(user);

        var cardRepository = Substitute.For<ICardRepository>();
        cardRepository.ListByUserId(user.UserId).Returns(new List<Card> { card1, card2 });

        var cardService = new CardService(cardRepository, userRepository);

        // Act
        cardService.Create(user.UserId, CardType.Physical);

        // Assert
        cardRepository.Received(1).Create(Arg.Is<Card>(item => item.Type == CardType.Physical));
    }

    [Fact]
    public void ShouldThrowExceptionWhenUserIsBlocked()
    {
        // Arrange
        var user = new User("123", "Astrid", UserStatus.Blocked);

        var userRepository = Substitute.For<IUserRepository>();
        userRepository.Fetch(user.UserId).Returns(user);

        var cardRepository = Substitute.For<ICardRepository>();
        var cardService = new CardService(cardRepository, userRepository);

        // Act

        // Assert
        Assert.Throws<CannotPhysicalCardException>(() => cardService.Create(user.UserId, CardType.Physical));
    }

    [Fact]
    public void ShouldThrowExceptionWhenUserIsNotFound()
    {
        // Arrange
        var userId = "123";

        var userRepository = Substitute.For<IUserRepository>();
        userRepository.Fetch(userId).Throws(new UserNotFoundException());

        var cardRepository = Substitute.For<ICardRepository>();
        var cardService = new CardService(cardRepository, userRepository);

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

        var userRepository = Substitute.For<IUserRepository>();
        userRepository.Fetch(user.UserId).Returns(user);

        var cardRepository = Substitute.For<ICardRepository>();
        cardRepository.ListByUserId(user.UserId).Returns(new List<Card> { card });

        var cardService = new CardService(cardRepository, userRepository);

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

        var userRepository = Substitute.For<IUserRepository>();
        userRepository.Fetch(user.UserId).Returns(user);

        var cardRepository = Substitute.For<ICardRepository>();
        cardRepository.ListByUserId(user.UserId).Returns(new List<Card> { card });

        var cardService = new CardService(cardRepository, userRepository);

        // Act
        cardService.Create(user.UserId, CardType.Virtual);


        // Assert
        cardRepository.Received(1).Create(Arg.Is<Card>(item => item.Type == CardType.Virtual));

    }
}
