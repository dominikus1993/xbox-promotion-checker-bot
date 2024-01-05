using System.Runtime.CompilerServices;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Filters;

public sealed class GamePriceFilter : IGamesFilter
{
    private readonly PromotionPercentage _minimumPercentage;

    public GamePriceFilter(PromotionPercentage minimumPercentage)
    {
        _minimumPercentage = minimumPercentage;
    }
    
    public async IAsyncEnumerable<XboxGame> Filter(IAsyncEnumerable<XboxGame> games, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(games);
        await foreach (var game in games.WithCancellation(cancellationToken))
        {
            if (game.PromotionPercentage >= _minimumPercentage)
            {
                yield return game;
            }
        }
    }
}