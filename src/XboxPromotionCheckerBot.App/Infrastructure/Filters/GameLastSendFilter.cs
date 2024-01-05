using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Infrastructure.Filters;

public sealed class GameLastSendFilter : IGamesFilter
{
    public IAsyncEnumerable<XboxGame> Filter(IAsyncEnumerable<XboxGame> games, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}