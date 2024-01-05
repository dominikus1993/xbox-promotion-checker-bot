using AutoFixture.Xunit2;
using XboxPromotionCheckerBot.App.Core.Extensions;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Tests.Core.Extensions;

public class AsyncEnumerableExtensionsTests
{
    [Theory]
    [AutoData]
    public async Task TestWhenFilterEnumerableIsEmpty(IEnumerable<Game> games)
    {
        var filters = Enumerable.Empty<IGamesFilter>();

        var result = await games.ToAsyncEnumerable().Pipe(filters).ToListAsync();
        
        Assert.Equivalent(games, result);
    }
}