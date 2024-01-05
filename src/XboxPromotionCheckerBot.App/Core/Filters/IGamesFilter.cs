using System.Collections.Generic;
using System.Threading;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Filters;

public interface IGamesFilter
{
    IAsyncEnumerable<XboxGame> Filter(IAsyncEnumerable<XboxGame> games, CancellationToken cancellationToken = default);
}