using AutoFixture.Xunit2;
using XboxPromotionCheckerBot.App.Core.Repositories;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Tests.Infrastructure.Fixtures;

namespace XboxPromotionCheckerBot.App.Tests.Infrastructure.Repositories;

public class MongoGamesRepositoryTests : IClassFixture<MongoDbFixture>
{
    private readonly MongoDbFixture _mongoDbFixture;
    private readonly IGamesRepository _gamesRepository;
    public MongoGamesRepositoryTests(MongoDbFixture mongoDbFixture)
    {
        _mongoDbFixture = mongoDbFixture;
        _gamesRepository = _mongoDbFixture.MongoGamesRepository;
    }

    [Theory]
    [AutoData]
    public async Task TestWhenRecordNotExists(XboxGame game)
    {
        var exists = await _gamesRepository.Exists(game);
        Assert.False(exists);
    }
    
    [Theory]
    [AutoData]
    public async Task TestWhenRecordExists(XboxGame game)
    {
        await _gamesRepository.Insert(game);
        
        var exists = await _gamesRepository.Exists(game);
        Assert.True(exists);
    }
    
    [Theory]
    [AutoData]
    public async Task TestInsertManyAndSearchWhenRecordExists(XboxGame[] games)
    {
        await _gamesRepository.Insert(games);

        var randGame = Random.Shared.GetItems(games, 1).First();
        
        var exists = await _gamesRepository.Exists(randGame);
        Assert.True(exists);
    }
}