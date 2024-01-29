using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using XboxPromotionCheckerBot.App.Core.Extensions;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Notifications;
using XboxPromotionCheckerBot.App.Core.Providers;

namespace XboxPromotionCheckerBot.App.Core.UseCases;

public sealed class ParseGamesFilterAndSendItUseCase
{
    private readonly IGamesParser _gamesParser;
    private readonly IEnumerable<IGamesFilter> _gamesFilters;
    private readonly IGamesBroadcaster _gamesBroadcaster;

    public ParseGamesFilterAndSendItUseCase(IGamesParser gamesParser, IEnumerable<IGamesFilter> gamesFilters, IGamesBroadcaster gamesBroadcaster)
    {
        _gamesParser = gamesParser;
        _gamesFilters = gamesFilters;
        _gamesBroadcaster = gamesBroadcaster;
    }

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        var games = _gamesParser.Parse(cancellationToken);
        
        games = games.Pipe(_gamesFilters, cancellationToken);

        await _gamesBroadcaster.Broadcast(games, cancellationToken);
    }
}