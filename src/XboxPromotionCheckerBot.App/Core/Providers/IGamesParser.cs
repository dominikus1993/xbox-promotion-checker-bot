using System.Collections.Generic;
using System.Threading;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Providers;

public interface IGamesParser
{
    IAsyncEnumerable<XboxGame> Parse(CancellationToken cancellationToken = default);
}