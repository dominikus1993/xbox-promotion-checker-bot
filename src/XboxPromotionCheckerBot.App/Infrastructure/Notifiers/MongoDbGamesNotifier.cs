using Microsoft.Extensions.Logging;
using XboxPromotionCheckerBot.App.Core.Notifications;
using XboxPromotionCheckerBot.App.Core.Repositories;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Infrastructure.Logger;

namespace XboxPromotionCheckerBot.App.Infrastructure.Notifiers;

public sealed class MongoDbGamesNotifier : IGamesNotifier
{
    private readonly IGamesRepository _gamesRepository;
    private readonly ILogger<MongoDbGamesNotifier> _logger;
    public MongoDbGamesNotifier(IGamesRepository gamesRepository, ILogger<MongoDbGamesNotifier> logger)
    {
        _gamesRepository = gamesRepository;
        _logger = logger;
    }

    public async Task Notify(IReadOnlyList<XboxGame> games, CancellationToken cancellationToken = default)
    {
        _logger.LogSaveGamesToDatabase();
        await _gamesRepository.Insert(games, cancellationToken);
        _logger.LogGamesSavedToDatabase();
    }
}