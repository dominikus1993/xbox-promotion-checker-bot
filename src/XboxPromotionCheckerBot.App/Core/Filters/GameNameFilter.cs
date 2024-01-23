using XboxPromotionCheckerBot.App.Core.Repositories;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Filters;

public sealed class GameNameFilter : IGamesFilter
{
    private readonly IGameSearcher _gameSearcher;

    public GameNameFilter(IGameSearcher gameSearcher)
    {
        _gameSearcher = gameSearcher;
    }

    public IAsyncEnumerable<XboxGame> Filter(IAsyncEnumerable<XboxGame> games, CancellationToken cancellationToken = default)
    {
        return _gameSearcher.FilterExistingGames(games, cancellationToken);
    }
}