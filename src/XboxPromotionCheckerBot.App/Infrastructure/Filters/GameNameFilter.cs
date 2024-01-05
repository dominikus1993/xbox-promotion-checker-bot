using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Infrastructure.Filters;

public sealed class GameNameFilter : IGamesFilter
{
    public IAsyncEnumerable<XboxGame> Filter(IAsyncEnumerable<XboxGame> games, CancellationToken cancellationToken = default)
    {
        return games;
    }
}