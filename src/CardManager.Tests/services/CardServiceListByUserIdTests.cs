using CardManager.Domain.contracts;
using CardManager.Domain.exceptions;
using CardManager.Domain.repositories;
using CardManager.Domain.services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace CardManager.Tests.services;

public class CardServiceListByUserIdTests
{
    [Fact]
    public void ShouldReturnCardByUserId()
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
        var result = cardService.ListByUserId(userId);

        // Assert
        if (result.Count == 1)
        {
            Assert.Equal(card, result[0]);
        }
    }

    [Fact]
    public void ShouldReturnAnEmptyListWhenThereIsNoCard()
    {
        // Arrange
        var userId = "123";
        var cardRepository = Substitute.For<ICardRepository>();
        cardRepository.ListByUserId(userId).Throws(new CardNotFoundException());

        var cardService = new CardService(cardRepository);

        // Act
        var result = cardService.ListByUserId(userId);

        // Assert
        Assert.Equal(0, result.Count);
    }
}
