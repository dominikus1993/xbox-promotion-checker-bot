using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Tests.Core.Filters;

public class GamePriceFilterTests
{
    private readonly IGamesFilter _priceFilter;

    public GamePriceFilterTests()
    {
        _priceFilter = new GamePriceFilter(new PromotionPercentage(40d));
    }
    
    [Fact]
    public async Task TestPriceFilterWhenSourceIsNull()
    {
        var subject = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _priceFilter.Filter(null).ToArrayAsync());
        Assert.NotNull(subject);
    }
    
    [Fact]
    public async Task TestPriceFilterWhenSourceIsEmpty()
    {
        var games = AsyncEnumerable.Empty<XboxGame>();
        var subject = await _priceFilter.Filter(games).ToArrayAsync();
        Assert.Empty(subject);
    }
    
    [Fact]
    public async Task TestPriceFilterWhenSourceIsNotEmpty()
    {
        var game = XboxGame.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(10, 20));
        var games = new[] { game, }.ToAsyncEnumerable();
        var subject = await _priceFilter.Filter(games).ToArrayAsync();
        Assert.NotEmpty(subject);
        Assert.Single(subject);
        Assert.Equivalent(game, subject[0]);
    }
    
    [Fact]
    public async Task TestPriceFilterWhenSourceIsNotEmptyAndSomeGameHasSmallerPromotionThan40Percent()
    {
        var game = XboxGame.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(10, 20));
        var game2 = XboxGame.Create(Guid.NewGuid(), "x", new Uri("http://test.pl"), new GamePrice(19, 20));
        var games = new[] { game, game2}.ToAsyncEnumerable();
        var subject = await _priceFilter.Filter(games).ToArrayAsync();
        Assert.NotEmpty(subject);
        Assert.Single(subject);
        Assert.Equivalent(game, subject[0]);
    }
}