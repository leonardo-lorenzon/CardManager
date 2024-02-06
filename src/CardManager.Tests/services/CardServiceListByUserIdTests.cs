using CardManager.Domain.contracts;
using CardManager.Tests.builders;
using CardManager.Tests.factories;
using Xunit;

namespace CardManager.Tests.services;

public class CardServiceListByUserIdTests
{
    [Fact]
    public void ShouldReturnCardByUserId()
    {
        // Arrange
        var userId = "123";
        var card = new CardBuilder().WithUserId(userId).Build();

        var factory = new CardServiceTestFactory();
        factory.AddCards(new List<Card> { card });

        var cardService = factory.Build();

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
        var factory = new CardServiceTestFactory();

        var cardService = factory.Build();

        // Act
        var result = cardService.ListByUserId(userId);

        // Assert
        Assert.Equal(0, result.Count);
    }
}
