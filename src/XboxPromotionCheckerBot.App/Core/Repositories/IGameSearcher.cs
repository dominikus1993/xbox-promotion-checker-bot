using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Repositories;

public interface IGameSearcher : IDisposable
{
    IAsyncEnumerable<XboxGame> FilterExistingGames(IAsyncEnumerable<XboxGame> games,
        CancellationToken cancellationToken = default);
}