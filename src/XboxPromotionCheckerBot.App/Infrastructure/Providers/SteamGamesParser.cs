using System.Text.Json.Serialization;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Providers;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Infrastructure.Providers;

internal sealed class SteamAppList
{
    [JsonPropertyName("apps")] public List<SteamApp> Apps { get; set; }
}

internal sealed class SteamAppResponse
{
    [JsonPropertyName("applist")] public SteamAppList AppList { get; set; }
}

internal sealed class SteamApp
{
    [JsonPropertyName("appid")] public int AppId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
}

public sealed class SteamGamesParser : IGamesParser
{
    private readonly HttpClient _httpClient;
    private readonly GameNameFilter _gameNameFilter;

    public SteamGamesParser(HttpClient httpClient, GameNameFilter gameNameFilter)
    {
        _httpClient = httpClient;
        _gameNameFilter = gameNameFilter;
    }

    public IAsyncEnumerable<Game> Parse(CancellationToken cancellationToken = default)
    {
        var apps = _gameNameFilter.FilterSteamApps(GetAppList(cancellationToken), cancellationToken);

        return GetGames(apps, cancellationToken);
    }

    private IAsyncEnumerable<SteamApp> GetAppList(CancellationToken cancellationToken = default)
    {
        return AsyncEnumerable.Empty<SteamApp>();
    }

    private IAsyncEnumerable<Game> GetGames(IAsyncEnumerable<SteamApp> apps,
        CancellationToken cancellationToken = default)
    {
        return AsyncEnumerable.Empty<Game>();
    }
}