using CardManager.Domain.contracts;
using CardManager.Domain.exceptions;
using CardManager.Domain.repositories;
using CardManager.Domain.services;
using NSubstitute;
using Xunit;

namespace CardManager.Tests.services;

public class CardServiceCreateTests
{
    [Fact]
    public void ShouldCallCreateFromRepositoryWithUserId()
    {
        // Arrange
        var userId = "123";

        var cardRepository = Substitute.For<ICardRepository>();

        var cardService = new CardService(cardRepository);

        // Act
        cardService.Create(userId, CardType.Physical);

        // Assert
        cardRepository.Received(1).Create(Arg.Any<Card>());
    }

    [Fact]
    public void ShouldThrowExceptionWhenTryToCreateMoreThanOnePhysicalCard()
    {
        // Arrange
        var userId = "123";

        var card = new Card(
            "111",
            userId,
            CardStatus.Issued,
            CardType.Physical,
            "5555111122223333",
            123,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        var cardRepository = Substitute.For<ICardRepository>();
        cardRepository.ListByUserId(userId).Returns(new List<Card> { card });

        var cardService = new CardService(cardRepository);

        // Act
        // Assert
        Assert.Throws<DuplicatedPhysicalCardException>(() => cardService.Create(userId, CardType.Physical));
    }

    [Fact]
    public void ShouldBeAbleToCreateMoreThanOneVirtualCard()
    {
        // Arrange
        var userId = "123";

        var card = new Card(
            "111",
            userId,
            CardStatus.Issued,
            CardType.Virtual,
            "5555111122223333",
            123,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        var cardRepository = Substitute.For<ICardRepository>();
        cardRepository.ListByUserId(userId).Returns(new List<Card> { card });

        var cardService = new CardService(cardRepository);

        // Act


        // Assert
        try
        {
            cardService.Create(userId, CardType.Virtual);
        }
        catch (DuplicatedPhysicalCardException)
        {
            Assert.True(false);
        }

    }
}
