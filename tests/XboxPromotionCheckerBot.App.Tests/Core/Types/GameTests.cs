using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Tests.Core.Types;

public class GameTests
{
    [Theory]
    [ClassData(typeof(PromotionPercentageData))]
    public void TestPromotionPercentage(Game game, PromotionPercentage promotion)
    {
        var subject = game.PromotionPercentage();
        
        Assert.Equal(promotion, subject);
    }
}

public sealed class PromotionPercentageData : TheoryData<Game, PromotionPercentage>
{
    public PromotionPercentageData()
    {
        Add(Game.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(300, 200),"xbox"), new PromotionPercentage(0d));
        Add(Game.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(10, 20), "xbox"), new PromotionPercentage(50d));
        Add(Game.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(99, 100), "xbox"), new PromotionPercentage(1d));
        Add(Game.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(10), "xbox"), PromotionPercentage.Zero);
        Add(Game.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(10, 100), "xbox"), new PromotionPercentage(90d));
        Add(Game.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(134.99m, 269.99m), "xbox"), new PromotionPercentage(50d));
        Add(Game.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(0, 0), "xbox"), new PromotionPercentage(0));
    }
}