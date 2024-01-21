using CardManager.Domain.contracts;
using CardManager.Domain.services;
using CardManager.Tests.doubles.faker;
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

        var cardRepository = new InMemoryCardRepository();
        cardRepository.Create(card);
        var userRepository = new InMemoryUserRepository();

        var cardService = new CardService(cardRepository, userRepository);

        // Act
        var result = cardService.ListByUserId(userId);

        // Assert
        Assert.Equal(1, result.Count);
        Assert.Equal(card, result[0]);
    }

    [Fact]
    public void ShouldReturnAnEmptyListWhenThereIsNoCard()
    {
        // Arrange
        var userId = "123";
        var cardRepository = new InMemoryCardRepository();
        var userRepository = new InMemoryUserRepository();

        var cardService = new CardService(cardRepository, userRepository);

        // Act
        var result = cardService.ListByUserId(userId);

        // Assert
        Assert.Equal(0, result.Count);
    }
}
