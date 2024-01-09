using CardManager.Domain.repositories;
using CardManager.Domain.services;
using CardManager.Infrastructure.repositories;

namespace CardManager.Api;

public class DependencyInversion
{
    private readonly IServiceCollection _serviceCollection;

    public DependencyInversion(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public void AddServices() => _serviceCollection.AddTransient<ICardService, CardService>();

    public void AddRepositories() => _serviceCollection.AddSingleton<ICardRepository, CardRepository>();
}
