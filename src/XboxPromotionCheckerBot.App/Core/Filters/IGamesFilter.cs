using System.Collections.Generic;
using System.Threading;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Filters;

public interface IGamesFilter
{
    IAsyncEnumerable<Game> Filter(IAsyncEnumerable<Game> games, CancellationToken cancellationToken = default);
}