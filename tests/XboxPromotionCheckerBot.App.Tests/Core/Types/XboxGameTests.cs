using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Tests.Core.Types;

public class XboxGameTests
{
    [Theory]
    [ClassData(typeof(PromotionPercentageData))]
    public void TestPromotionPercentage(XboxGame game, PromotionPercentage promotion)
    {
        var subject = game.PromotionPercentage();
        
        Assert.Equal(promotion, subject);
    }
}

public sealed class PromotionPercentageData : TheoryData<XboxGame, PromotionPercentage>
{
    public PromotionPercentageData()
    {
        Add(XboxGame.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(300, 200)), new PromotionPercentage(0d));
        Add(XboxGame.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(10, 20)), new PromotionPercentage(50d));
        Add(XboxGame.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(99, 100)), new PromotionPercentage(1d));
        Add(XboxGame.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(10)), PromotionPercentage.Zero);
        Add(XboxGame.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(10, 100)), new PromotionPercentage(90d));
        Add(XboxGame.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(134.99m, 269.99m)), new PromotionPercentage(50d));
    }
}