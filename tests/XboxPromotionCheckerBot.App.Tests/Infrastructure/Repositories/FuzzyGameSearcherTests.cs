using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Infrastructure.Repositories;

namespace XboxPromotionCheckerBot.App.Tests.Infrastructure.Repositories;

public sealed class FuzzyGameSearcherTests
{
    [Fact]
    public async Task TestWhenGameExists()
    {
        FuzzGame[] games = [new FuzzGame("Elden Ring"), new FuzzGame("skyrim"), new FuzzGame("stellaris")];
        using var searcher = FuzzyGameSearcher.Create(games);

        var subject = await searcher.FilterExistingGames(new []{ XboxGame.Create("Stellaris Enchanced", new Uri("http://localhost"), new GamePrice(10, 20))}.ToAsyncEnumerable()).ToListAsync();
        
        Assert.NotEmpty(subject);
    }
    
    [Fact]
    public async Task TestWhenGameExistsAndNotExistsInSecondQuery()
    {
        FuzzGame[] games = [new FuzzGame("Elden Ring"), new FuzzGame("skyrim"), new FuzzGame("cyberpunk")];
        using var searcher = FuzzyGameSearcher.Create(games);

        var subject = await searcher.FilterExistingGames(new []{ XboxGame.Create("Stellaris Enchanced", new Uri("http://localhost"), new GamePrice(10, 20)), XboxGame.Create("Cyberpunk 2077 Complete Edition", new Uri("http://localhost"), new GamePrice(10, 20))}.ToAsyncEnumerable()).ToListAsync();
        
        Assert.Single(subject);
    }
    
    [Fact]
    public async Task TestWhenGameNotExists()
    {
        FuzzGame[] games = [new FuzzGame("Elden Ring"), new FuzzGame("The Elder Scroll V"), new FuzzGame("Stellaris Enchanced")];
        using var searcher = FuzzyGameSearcher.Create(games);

        var subject = await searcher.FilterExistingGames(new []{XboxGame.Create("Cyberpunk 2077 Complete Edition", new Uri("http://localhost"), new GamePrice(10, 20))}.ToAsyncEnumerable()).ToListAsync();
        
        Assert.Empty(subject);
    }
}