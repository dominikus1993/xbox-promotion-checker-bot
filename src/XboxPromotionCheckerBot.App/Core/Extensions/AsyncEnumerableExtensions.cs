using System.Collections.Generic;
using System.Threading;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Extensions;

public static class AsyncEnumerableExtensions
{
    public static IAsyncEnumerable<XboxGame> Pipe(this IAsyncEnumerable<XboxGame> games, IEnumerable<IGamesFilter> filters,
        CancellationToken cancellationToken = default)
    {
        var stream = games;
        foreach (var filter in filters)
        {
            stream = filter.Filter(stream, cancellationToken);
        }

        return stream;
    } 
}