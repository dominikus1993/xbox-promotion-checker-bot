using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Repositories;

public interface IGamesRepository
{
    Task<bool> Exists(XboxGame game, CancellationToken cancellationToken = default);
    Task Insert(XboxGame game, CancellationToken cancellationToken = default);
}