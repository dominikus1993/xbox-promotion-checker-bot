using Testcontainers.MongoDb;
using XboxPromotionCheckerBot.App.Infrastructure.MongoDb;
using XboxPromotionCheckerBot.App.Infrastructure.Repositories;

namespace XboxPromotionCheckerBot.App.Tests.Infrastructure.Fixtures;

public sealed class MongoDbFixture : IAsyncLifetime
{
    public MongoDbContainer MongoDbContainer = new MongoDbBuilder().Build();
    
    public MongoGamesRepository MongoGamesRepository { get; private set; }

    public MongoDbFixture()
    {
        
    }


    public async Task InitializeAsync()
    {
        await MongoDbContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return MongoDbContainer.StopAsync();
    }
}