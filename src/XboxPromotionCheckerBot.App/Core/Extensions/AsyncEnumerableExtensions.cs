using System.Collections.Generic;
using System.Threading;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Providers;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Extensions;

public static class AsyncEnumerableExtensions
{
    public static IAsyncEnumerable<XboxGame> Pipe(this IAsyncEnumerable<XboxGame> games, IEnumerable<IGamesFilter> filters,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(games);
        ArgumentNullException.ThrowIfNull(filters);
        
        var stream = games;
        foreach (var filter in filters)
        {
            stream = filter.Filter(stream, cancellationToken);
        }

        return stream;
    }
    
    public static IAsyncEnumerable<XboxGame> MergeStreams(this IEnumerable<IAsyncEnumerable<XboxGame>> games,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(games);
        var streams = games.ToArray();

        return AsyncEnumerableEx.Merge(streams);
    }
    
    public static IAsyncEnumerable<XboxGame> MergeStreams(this IEnumerable<IGamesParser> games,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(games);
        var streams = games.Select(x => x.Parse(cancellationToken)).ToArray();
        
        return AsyncEnumerableEx.Merge(streams);
    }
}