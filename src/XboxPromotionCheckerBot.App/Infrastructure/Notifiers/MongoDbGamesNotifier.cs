using XboxPromotionCheckerBot.App.Core.Notifications;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Infrastructure.Notifiers;

public sealed class MongoDbGamesNotifier : IGamesNotifier
{
    public Task Notify(IReadOnlyList<XboxGame> games, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}