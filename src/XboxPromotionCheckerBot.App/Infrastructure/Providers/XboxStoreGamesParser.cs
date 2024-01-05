using XboxPromotionCheckerBot.App.Core.Providers;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Infrastructure.Providers;

public sealed class XboxStoreGamesParser : IGamesParser
{
    public IAsyncEnumerable<Game> Parse(CancellationToken cancellationToken = default)
    {
        return AsyncEnumerable.Empty<Game>();
    }
}