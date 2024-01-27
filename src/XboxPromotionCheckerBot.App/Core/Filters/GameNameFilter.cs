using XboxPromotionCheckerBot.App.Core.Repositories;
using XboxPromotionCheckerBot.App.Core.Types;
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

    public bool Contains(XboxGame game)
    {
        var title = game.Title.Normalize().ToUpperInvariant();
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

    public IAsyncEnumerable<XboxGame> Filter(IAsyncEnumerable<XboxGame> games, CancellationToken cancellationToken = default)
    {
        return games.Where(x =>
        {
            return _games.Any(game => game.Contains(x));
        });
    }
}