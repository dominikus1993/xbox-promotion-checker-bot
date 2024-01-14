using Microsoft.Extensions.Logging;

namespace XboxPromotionCheckerBot.App.Infrastructure.Logger;

public static partial class InfrastructureLogging
{
    [LoggerMessage(LogLevel.Information, "Save games to database")]
    public static partial void LogSaveGamesToDatabase(this ILogger logger);
    
    [LoggerMessage(LogLevel.Information, "Games saved to database")]
    public static partial void LogGamesSavedToDatabase(this ILogger logger);

    [LoggerMessage(LogLevel.Warning, "No games in page {Page}")]
    public static partial void LogNoGames(this ILogger logger, int page);
    
    [LoggerMessage(LogLevel.Warning, "Can't find prices, {InnerText}")]
    public static partial void LogCantFindPrices(this ILogger logger, string innerText);
    
    [LoggerMessage(LogLevel.Warning, "Can't parse url {Uri}")]
    public static partial void LogCantParseUrl(this ILogger logger, string uri);
}