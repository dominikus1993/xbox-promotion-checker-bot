using System.Runtime.CompilerServices;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Infrastructure.Repositories;

namespace XboxPromotionCheckerBot.App.Infrastructure.Filters;

public sealed class GameLastSendFilter : IGamesFilter
{
    private readonly MongoGamesRepository _mongoGamesRepository;

    public GameLastSendFilter(MongoGamesRepository mongoGamesRepository)
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