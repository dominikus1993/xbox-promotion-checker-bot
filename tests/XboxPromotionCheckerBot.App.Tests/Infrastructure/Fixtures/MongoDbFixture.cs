using Testcontainers.MongoDb;
using XboxPromotionCheckerBot.App.Core.Repositories;
using XboxPromotionCheckerBot.App.Infrastructure.MongoDb;
using XboxPromotionCheckerBot.App.Infrastructure.Repositories;

namespace XboxPromotionCheckerBot.App.Tests.Infrastructure.Fixtures;

public sealed class MongoDbFixture : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder().Build();
    
    public IGamesRepository MongoGamesRepository { get; private set; }

    
    public MongoDbFixture()
    {
        
    }


    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
        var client = MongoDbSetup.MongoClient(_mongoDbContainer.GetConnectionString());
        var db = client.GamesDb();
        await db.Setup();
        MongoGamesRepository = new MongoGamesRepository(db);
    }

    public Task DisposeAsync()
    {
        return _mongoDbContainer.StopAsync();
    }
}