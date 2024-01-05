using System.Runtime.CompilerServices;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Infrastructure.Filters;

public sealed class GamePriceFilter : IGamesFilter
{
    public async IAsyncEnumerable<Game> Filter(IAsyncEnumerable<Game> games, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var game in games.WithCancellation(cancellationToken))
        {
            if (game.PromotionPercentage > 40)
            {
                yield return game;
            }
        }
    }
}