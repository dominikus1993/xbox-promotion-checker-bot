using System.Runtime.CompilerServices;
using XboxPromotionCheckerBot.App.Core.Repositories;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Filters;

public sealed class GameLastSendFilter : IGamesFilter
{
    private readonly IGamesRepository _mongoGamesRepository;

    public GameLastSendFilter(IGamesRepository mongoGamesRepository)
    {
        _mongoGamesRepository = mongoGamesRepository;
    }


    public async IAsyncEnumerable<XboxGame> Filter(IAsyncEnumerable<XboxGame> games, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var game in games.WithCancellation(cancellationToken))
        {
            var exists = await _mongoGamesRepository.Exists(game, cancellationToken);
            if (!exists)
            {
                yield return game;
            }
        }
    }
}