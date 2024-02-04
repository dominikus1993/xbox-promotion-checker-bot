using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Providers;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Infrastructure.Providers;

internal sealed class SteamAppList
{
    [JsonPropertyName("apps")] public List<SteamApp>? Apps { get; set; }
}

internal sealed class SteamAppResponse
{
    [JsonPropertyName("applist")] public SteamAppList? AppList { get; set; }
}

internal sealed class SteamApp
{
    [JsonPropertyName("appid")] public int AppId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
}

public sealed class SteamGamesParser : IGamesParser
{
    private const string Platform = "steam";
    private readonly HttpClient _httpClient;
    private readonly GameNameFilter _gameNameFilter;
    private readonly ILogger<SteamGamesParser> _logger;

    public SteamGamesParser(HttpClient httpClient, GameNameFilter gameNameFilter, ILogger<SteamGamesParser> logger)
    {
        _httpClient = httpClient;
        _gameNameFilter = gameNameFilter;
        _logger = logger;
    }

    public IAsyncEnumerable<Game> Parse(CancellationToken cancellationToken = default)
    {
        var apps = GetAppList(cancellationToken);

        return GetGames(apps, cancellationToken);
    }

    private async IAsyncEnumerable<SteamApp> GetAppList(CancellationToken cancellationToken = default)
    {
        var result =
            await _httpClient.GetAsync("http://api.steampowered.com/ISteamApps/GetAppList/v2/", cancellationToken: cancellationToken);

        if (!result.IsSuccessStatusCode)
        {
            _logger.LogWarning("Failed to get app list from Steam, {StatusCode}, {ReasonPhrase}", result.StatusCode, result.ReasonPhrase);
            yield break;
        }
        
        var response = await result.Content.ReadFromJsonAsync<SteamAppResponse>(cancellationToken: cancellationToken);

        if (response?.AppList?.Apps is null or { Count: 0 })
        {
            _logger.LogWarning("No apps found in the response");
            yield break;
        }
        
        foreach (var app in _gameNameFilter.FilterSteamApps(response.AppList.Apps))
        {
            yield return app;
        }
    }

    private async IAsyncEnumerable<Game> GetGames(IAsyncEnumerable<SteamApp> apps,
        CancellationToken cancellationToken = default)
    {
        await foreach (var app in apps.WithCancellation(cancellationToken))
        {
            var game = await GetGame(app, cancellationToken);
            if (game is not null)
            {
                yield return game;
            }
        }
    }

    private async Task<Game?> GetGame(SteamApp app, CancellationToken cancellationToken = default)
    {
        var appId = app.AppId.ToString();
        var details = await _httpClient.GetAsync($"https://store.steampowered.com/api/appdetails?appids={appId}&cc=PLN",
            cancellationToken: cancellationToken);

        if (!details.IsSuccessStatusCode)
        {
            _logger.LogWarning("Failed to get details for app {AppId}, {StatusCode}, {ReasonPhrase}", app.AppId, details.StatusCode, details.ReasonPhrase);
            return null;
        }

        var result = await details.Content.ReadFromJsonAsync<Dictionary<string, SteamAppData>>(cancellationToken);

        if (result is null || !result.TryGetValue(appId, out var data) || !data.Success)
        {
            _logger.LogWarning("Failed to get details for app {AppId}", app.AppId);
            return null;
        }
        
        var price = ParsePrice(data.Data.PriceOverview);

        if (!price.HasValue)
        {
            _logger.LogWarning("Failed to parse price for app {AppId}", app.AppId);
            return null;
        }
        
        return Game.Create(data.Data.Name, new Uri($"https://store.steampowered.com/app/{app.AppId}"), price.Value, Platform);
    }

    private static GamePrice? ParsePrice(PriceOverview? overview)
    {
        if (overview is null)
        {
            return null;
        }
        
        var price = overview.FinalFormatted?.Replace("zł", string.Empty, StringComparison.InvariantCultureIgnoreCase).Replace(",", ".", StringComparison.InvariantCultureIgnoreCase);
        if (!string.IsNullOrEmpty(price) && decimal.TryParse(price, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var value))
        {
            var oldPrice = overview.InitialFormatted?.Replace("zł", string.Empty, StringComparison.InvariantCultureIgnoreCase).Replace(",", ".", StringComparison.InvariantCultureIgnoreCase);
            if (!string.IsNullOrEmpty(oldPrice) && decimal.TryParse(oldPrice, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var oldValue))
            {
                return new GamePrice(value, oldValue);
            }

            return new GamePrice(value);
        }

        return null;
    }
}