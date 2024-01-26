using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Core.Notifications;

public interface IGamesNotifier
{
    Task Notify(IReadOnlyList<XboxGame> games, CancellationToken cancellationToken = default);
}

public interface IGamesBroadcaster
{
    Task Broadcast(IAsyncEnumerable<XboxGame> games, CancellationToken cancellationToken = default);
}

public sealed class DefaultGamesBroadcaster : IGamesBroadcaster
{
    private readonly IEnumerable<IGamesNotifier> _gamesNotifiers;

    public DefaultGamesBroadcaster(IEnumerable<IGamesNotifier> gamesNotifiers)
    {
        _gamesNotifiers = gamesNotifiers;
    }

    public async Task Broadcast(IAsyncEnumerable<XboxGame> games, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var result = await games.ToArrayAsync(cancellationToken: cancellationToken);
        if (result is null or {Length: 0})
        {
            return;
        }

        List<Task> tasks = [];

        foreach (var notifier in _gamesNotifiers)
        {
            tasks.Add(notifier.Notify(result, cancellationToken));
        }

        await Task.WhenAll(tasks);
    }
}