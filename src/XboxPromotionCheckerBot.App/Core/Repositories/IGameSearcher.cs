using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Repositories;

public interface IGameSearcher : IDisposable
{
    IAsyncEnumerable<Game> FilterExistingGames(IAsyncEnumerable<Game> games,
        CancellationToken cancellationToken = default);
}