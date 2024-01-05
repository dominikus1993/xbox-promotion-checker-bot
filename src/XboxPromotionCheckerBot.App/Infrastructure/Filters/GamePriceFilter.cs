using System.Runtime.CompilerServices;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Infrastructure.Filters;

public sealed class GamePriceFilter : IGamesFilter
{
    private static readonly PromotionPercentage FortyPercent = new PromotionPercentage(40d);
    public async IAsyncEnumerable<XboxGame> Filter(IAsyncEnumerable<XboxGame> games, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var game in games.WithCancellation(cancellationToken))
        {
            if (game.PromotionPercentage > FortyPercent)
            {
                yield return game;
            }
        }
    }
}