using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Repositories;

public interface IGamesRepository
{
    Task<bool> Exists(Game game, CancellationToken cancellationToken = default);
    Task Insert(Game game, CancellationToken cancellationToken = default);
    Task Insert(IEnumerable<Game> game, CancellationToken cancellationToken = default);
}