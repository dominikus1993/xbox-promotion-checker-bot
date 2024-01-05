using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Providers;

namespace XboxPromotionCheckerBot.App.Core.UseCases;

public sealed class ParseGamesFilterAndSendItUseCase
{
    private readonly IGamesParser _gamesParser;
    private readonly IEnumerable<IGamesFilter> _gamesFilters;

    public ParseGamesFilterAndSendItUseCase(IGamesParser gamesParser, IEnumerable<IGamesFilter> gamesFilters)
    {
        _gamesParser = gamesParser;
        _gamesFilters = gamesFilters;
    }

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        var games = _gamesParser.Parse(cancellationToken);
        
    }
}