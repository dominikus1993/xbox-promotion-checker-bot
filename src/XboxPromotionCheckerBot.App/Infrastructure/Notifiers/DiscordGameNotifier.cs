using XboxPromotionCheckerBot.App.Core.Notifications;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Infrastructure.Notifiers;

public sealed class DiscordGameNotifier : IGamesNotifier
{
    public Task Notify(IReadOnlyList<Game> games, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}