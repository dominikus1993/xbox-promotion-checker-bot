using XboxPromotionCheckerBot.App.Core.Repositories;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Infrastructure.Repositories;

namespace XboxPromotionCheckerBot.App.Core.Filters;

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