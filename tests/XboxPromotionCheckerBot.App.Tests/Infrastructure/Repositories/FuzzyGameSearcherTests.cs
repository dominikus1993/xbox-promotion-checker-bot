using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Infrastructure.Repositories;

namespace XboxPromotionCheckerBot.App.Tests.Infrastructure.Repositories;

public sealed class FuzzyGameSearcherTests
{
    [Fact]
    public async Task TestWhenGameExists()
    {
        FuzzGame[] games = [new FuzzGame("Elden Ring"), new FuzzGame("skyrim"), new FuzzGame("stellaris")];
        var searcher = new GameNameFilter(games);

        var subject = await searcher.Filter(new []{ Game.Create("Stellaris Enchanced", new Uri("http://localhost"), new GamePrice(10, 20), "xbox")}.ToAsyncEnumerable()).ToListAsync();
        
        Assert.NotEmpty(subject);
    }
    
    [Fact]
    public async Task TestWhenGameExistsAndNotExistsInSecondQuery()
    {
        FuzzGame[] games = [new FuzzGame("Elden Ring"), new FuzzGame("skyrim"), new FuzzGame("cyberpunk")];
        var searcher = new GameNameFilter(games);

        var subject = await searcher.Filter(new []{ Game.Create("Stellaris Enchanced", new Uri("http://localhost"), new GamePrice(10, 20), "xbox"), Game.Create("Cyberpunk 2077 Complete Edition", new Uri("http://localhost"), new GamePrice(10, 20), "xbox")}.ToAsyncEnumerable()).ToListAsync();
        
        Assert.Single(subject);
    }
    
    [Fact]
    public async Task TestWhenGameNotExists()
    {
        FuzzGame[] games = [new FuzzGame("Elden Ring"), new FuzzGame("The Elder Scroll V"), new FuzzGame("Stellaris Enchanced")];
        var searcher = new GameNameFilter(games);

        var subject = await searcher.Filter(new []{Game.Create("Cyberpunk 2077 Complete Edition", new Uri("http://localhost"), new GamePrice(10, 20), "xbox")}.ToAsyncEnumerable()).ToListAsync();
        
        Assert.Empty(subject);
    }

    [Fact]
    public async Task TestParsingWhenGamesAreInGamesThatIWantBuy()
    {
        FuzzGame[] games = [new FuzzGame("stellaris"), new FuzzGame("cyberpunk")];
        var searcher = new GameNameFilter(games);
        Game[] gamesFromSomething = [Game.Create("Cyberpunk 2077", new Uri("http://localhost"), new GamePrice(10, 20), "xbox"), Game.Create("Stellaris Enchanced", new Uri("http://localhost"), new GamePrice(10, 20), "xbox"), Game.Create("Assasins Creed", new Uri("http://localhost"), new GamePrice(10, 20), "xbox"), Game.Create("STORY OF SEASONS: Friends of Mineral Town - Digital Edition", new Uri("http://localhost"), new GamePrice(10, 20), "xbox")];
        var subject = await searcher.Filter(gamesFromSomething.ToAsyncEnumerable()).ToListAsync();
        
        Assert.NotEmpty(subject);
        Assert.Equal(2, subject.Count);
        Assert.Contains(subject, game => game.Title == "Cyberpunk 2077");
        Assert.Contains(subject, game => game.Title == "Stellaris Enchanced");
    }
}