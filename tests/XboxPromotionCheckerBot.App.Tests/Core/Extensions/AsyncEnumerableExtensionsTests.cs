using AutoFixture.Xunit2;
using XboxPromotionCheckerBot.App.Core.Extensions;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Tests.Core.Extensions;

public class AsyncEnumerableExtensionsTests
{
    [Theory]
    [AutoData]
    public async Task TestWhenFilterEnumerableIsEmpty(IEnumerable<XboxGame> games)
    {
        var filters = Enumerable.Empty<IGamesFilter>();

        var result = await games.ToAsyncEnumerable().Pipe(filters).ToListAsync();
        
        Assert.Equivalent(games, result);
    }
    
    [Theory]
    [AutoData]
    public async Task TestWhenFilterEnumerableIsNull(IEnumerable<XboxGame> games)
    {
        var result = await Assert.ThrowsAsync<ArgumentNullException>(async () => await games.ToAsyncEnumerable().Pipe(null!).ToListAsync());
        
        Assert.NotNull(result);
    }
    
    [Theory]
    [AutoData]
    public async Task TestWhenGamesAreNull()
    {
        IAsyncEnumerable<XboxGame> games = null!;
        var filters = Enumerable.Empty<IGamesFilter>();
        var result = await Assert.ThrowsAsync<ArgumentNullException>(async () => await games.Pipe(filters).ToListAsync());
        
        Assert.NotNull(result);
    }
}