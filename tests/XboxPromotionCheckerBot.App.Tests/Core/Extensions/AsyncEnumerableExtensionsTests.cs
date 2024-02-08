using AutoFixture.Xunit2;
using XboxPromotionCheckerBot.App.Core.Extensions;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Tests.Core.Extensions;

internal sealed class Filter1 : IGamesFilter
{
    private readonly Func<Game, bool> _predicate;

    public Filter1(Func<Game, bool> predicate)
    {
        _predicate = predicate;
    }
    
    public async IAsyncEnumerable<Game> Filter(IAsyncEnumerable<Game> games, CancellationToken cancellationToken = default)
    {
        await foreach (var game in games.WithCancellation(cancellationToken))
        {
            if (_predicate(game))
            {
                yield return game;
            }
        }
    }
}

internal sealed class Filter2 : IGamesFilter
{
    private readonly Func<Game, bool> _predicate;

    public Filter2(Func<Game, bool> predicate)
    {
        _predicate = predicate;
    }

    public async IAsyncEnumerable<Game> Filter(IAsyncEnumerable<Game> games, CancellationToken cancellationToken = default)
    {
        await foreach (var game in games.WithCancellation(cancellationToken))
        {
            if (_predicate(game))
            {
                yield return game;
            }
        }
    }
}

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
    
    [Theory]
    [AutoData]
    public async Task TestWhenFilterEnumerableIsNull(IEnumerable<Game> games)
    {
        var result = await Assert.ThrowsAsync<ArgumentNullException>(async () => await games.ToAsyncEnumerable().Pipe(null!).ToListAsync());
        
        Assert.NotNull(result);
    }
    
    [Theory]
    [AutoData]
    public async Task TestWhenGamesAreNull()
    {
        IAsyncEnumerable<Game> games = null!;
        var filters = Enumerable.Empty<IGamesFilter>();
        var result = await Assert.ThrowsAsync<ArgumentNullException>(async () => await games.Pipe(filters).ToListAsync());
        
        Assert.NotNull(result);
    }
    
    [Theory]
    [AutoData]
    public async Task Test(IEnumerable<Game> games)
    {
        var filters = new IGamesFilter[] {new Filter1(_ => true), new Filter2(_ => true)};

        var result = await games.ToAsyncEnumerable().Pipe(filters).ToListAsync();
        
        Assert.Equivalent(games, result);
    }
}