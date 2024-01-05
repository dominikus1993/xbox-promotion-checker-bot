using System.Collections.Generic;
using System.Threading;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Providers;

public interface IGamesParser
{
    IAsyncEnumerable<Game> Parse(CancellationToken cancellationToken = default);
}