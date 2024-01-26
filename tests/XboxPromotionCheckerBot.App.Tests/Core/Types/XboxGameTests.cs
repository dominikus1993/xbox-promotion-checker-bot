using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Tests.Core.Types;

public class XboxGameTests
{
    [Theory]
    [MemberData(nameof(PromotionPercentageData))]
    public void TestPromotionPercentage(XboxGame game, PromotionPercentage promotion)
    {
        var subject = game.PromotionPercentage;
        
        Assert.Equal(promotion, subject);
    }
    
    
    
    public static IEnumerable<object[]> PromotionPercentageData =>
        new List<object[]>
        {
            new object[] { XboxGame.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(10, 20)), new PromotionPercentage(50d) },
            new object[] { XboxGame.Create(Guid.NewGuid(),"x", new Uri("http://test.pl"), new GamePrice(10)), PromotionPercentage.Zero },
            new object[] { XboxGame.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(10, 100)), new PromotionPercentage(90d) },
            new object[] { XboxGame.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(134.99m, 269.99m)), new PromotionPercentage(50d) },
        };
}