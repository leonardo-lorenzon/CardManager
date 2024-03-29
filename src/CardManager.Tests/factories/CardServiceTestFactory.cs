using CardManager.Domain.contracts;
using CardManager.Domain.services;
using CardManager.Tests.doubles.faker;

namespace CardManager.Tests.factories;

public class CardServiceTestFactory
{
    public InMemoryCardRepository CardRepository { get; }
    public InMemoryUserRepository UserRepository { get; }
    public InMemoryTimeService TimeService { get; }

    public CardServiceTestFactory()
    {
        CardRepository = new InMemoryCardRepository();
        UserRepository = new InMemoryUserRepository();
        TimeService = new InMemoryTimeService();
    }

    public CardService Build()
    {
        return new CardService(CardRepository, UserRepository, TimeService);
    }

    public void SetUser(User user)
    {
        UserRepository.AddUser(user);
    }

    public void AddCards(IList<Card> cards)
    {
        foreach (var card in cards)
        {
            CardRepository.Create(card);
        }
    }

    public void SetCurrentDate(DateTime currentDate)
    {
        TimeService.SetCurrentDate(currentDate);
    }
}
