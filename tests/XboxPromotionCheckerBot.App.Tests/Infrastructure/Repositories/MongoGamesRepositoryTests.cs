using AutoFixture.Xunit2;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Tests.Infrastructure.Fixtures;

namespace XboxPromotionCheckerBot.App.Tests.Infrastructure.Repositories;

public class MongoGamesRepositoryTests : IClassFixture<MongoDbFixture>
{
    private readonly MongoDbFixture _mongoDbFixture;

    public MongoGamesRepositoryTests(MongoDbFixture mongoDbFixture)
    {
        _mongoDbFixture = mongoDbFixture;
    }

    [Theory]
    [AutoData]
    public async Task TestWhenRecordNotExists(XboxGame game)
    {
        var exists = await _mongoDbFixture.MongoGamesRepository.Exists(game);
        Assert.False(exists);
    }
    
    [Theory]
    [AutoData]
    public async Task TestWhenRecordExists(XboxGame game)
    {
        await _mongoDbFixture.MongoGamesRepository.Insert(game);
        
        var exists = await _mongoDbFixture.MongoGamesRepository.Exists(game);
        Assert.True(exists);
    }
}