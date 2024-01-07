using XboxPromotionCheckerBot.App.Infrastructure.Providers;

namespace XboxPromotionCheckerBot.App.Tests.Infrastructure.Providers;

public class XboxStoreGamesParserTests
{
    [Fact]
    public async Task TestParse()
    {
        var parser = new XboxStoreGamesParser();

        var subject = await parser.Parse().ToListAsync();

        Assert.NotNull(subject);
        Assert.NotEmpty(subject);
        Assert.All(subject, game => Assert.True(game.IsValidGame()));
    }
}