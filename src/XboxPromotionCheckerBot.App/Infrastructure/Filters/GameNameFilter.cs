using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Infrastructure.Providers;

namespace XboxPromotionCheckerBot.App.Infrastructure.Filters;

public sealed record FuzzGame(Guid Id, string Title)
{
    private readonly string _normalizedTitle;
    public FuzzGame(string Title): this(Guid.NewGuid(), Title)
    {
        ArgumentException.ThrowIfNullOrEmpty(Title);
        _normalizedTitle = Title.Normalize().ToUpperInvariant();
    }

    public bool Contains(string? title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return false;
        }
        
        var normalizedTitle = title.Normalize().ToUpperInvariant();
        return normalizedTitle.Contains(_normalizedTitle, StringComparison.InvariantCultureIgnoreCase);
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
        return games.Where(x => Contains(x.Title));
    }

    private bool Contains(string? title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return false;
        }
        
        foreach (var game in _games.AsSpan())
        {
            if (game.Contains(title))
                return true;
        }

        return false;
    }
}