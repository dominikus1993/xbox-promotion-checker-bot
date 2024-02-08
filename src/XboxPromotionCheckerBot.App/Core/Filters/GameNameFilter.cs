using XboxPromotionCheckerBot.App.Core.Repositories;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Filters;

public sealed record FuzzGame(Guid Id, string Title)
{
    private readonly string _normalizedTitle;
    public FuzzGame(string Title): this(Guid.NewGuid(), Title)
    {
        ArgumentException.ThrowIfNullOrEmpty(Title);
        _normalizedTitle = Title.Normalize().ToUpperInvariant();
    }

    public bool Contains(Game game)
    {
        var title = game.Title.Normalize().ToUpperInvariant();
        return title.Contains(_normalizedTitle, StringComparison.InvariantCultureIgnoreCase);
    }
    
    internal bool Contains(string gameName)
    {
        if (string.IsNullOrEmpty(gameName))
        {
            return false;
        }
        
        var title = gameName.Normalize().ToUpperInvariant();
        return title.Contains(_normalizedTitle, StringComparison.InvariantCultureIgnoreCase);
    }
}

public sealed class GameNameFilter : IGamesFilter
{
    private readonly FuzzGame[] _games;
    
    public GameNameFilter(IEnumerable<FuzzGame> games)
    {
        _games = games.ToArray();
    }

    public IAsyncEnumerable<Game> Filter(IAsyncEnumerable<Game> games, CancellationToken cancellationToken = default)
    {
        return games.Where(x =>
        {
            foreach (var game in _games.AsSpan())
            {
                if (game.Contains(x)) 
                    return true;
            }

            return false;
        });
    }
    
    internal IEnumerable<T> FilterSteamApps<T>(IEnumerable<T> games, Func<T, FuzzGame, bool> contains)
    {
        foreach (var x in games)
        {
            foreach (var game in _games)
            {
                if (contains(x, game))
                    yield return x;
            }
            
        }
    }
}