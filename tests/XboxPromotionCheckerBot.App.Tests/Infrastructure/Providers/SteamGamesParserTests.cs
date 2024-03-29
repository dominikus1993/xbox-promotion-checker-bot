using Microsoft.Extensions.Logging.Abstractions;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Infrastructure.Filters;
using XboxPromotionCheckerBot.App.Infrastructure.Providers;

namespace XboxPromotionCheckerBot.App.Tests.Infrastructure.Providers;

public sealed class SteamGamesParserTests
{
    
    [Fact]
    public async Task TestParse()
    {
        using var client = new HttpClient();
        var parser = new SteamGamesParser(client, new GameNameFilter([new FuzzGame("cyberpunk 2077")]), NullLogger<SteamGamesParser>.Instance);
        var subject = await parser.Parse().ToListAsync();

        Assert.NotNull(subject);
        Assert.NotEmpty(subject);
        Assert.All(subject, game => Assert.True(game.IsValidGame()));
    }
}