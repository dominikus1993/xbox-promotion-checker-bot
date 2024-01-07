using Microsoft.Extensions.Logging.Abstractions;
using XboxPromotionCheckerBot.App.Infrastructure.Providers;

namespace XboxPromotionCheckerBot.App.Tests.Infrastructure.Providers;

public class XboxStoreGamesParserTests
{
    private readonly XboxStoreGamesParser _gamesParser;

    public XboxStoreGamesParserTests()
    {
        _gamesParser = new XboxStoreGamesParser(NullLogger<XboxStoreGamesParser>.Instance);
    }
    
    [Fact]
    public async Task TestParse()
    {
        var subject = await _gamesParser.Parse().ToListAsync();

        Assert.NotNull(subject);
        Assert.NotEmpty(subject);
        Assert.All(subject, game => Assert.True(game.IsValidGame()));
    }
}