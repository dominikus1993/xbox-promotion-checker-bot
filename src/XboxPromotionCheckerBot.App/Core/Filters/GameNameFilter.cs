using XboxPromotionCheckerBot.App.Core.Repositories;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Infrastructure.Providers;
using XboxPromotionCheckerBot.App.Infrastructure.Repositories;

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
    
    internal bool Contains(SteamApp game)
    {
        var title = game.Name.Normalize().ToUpperInvariant();
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
    
    internal IAsyncEnumerable<SteamApp> FilterSteamApps(IAsyncEnumerable<SteamApp> games, CancellationToken cancellationToken = default)
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
}