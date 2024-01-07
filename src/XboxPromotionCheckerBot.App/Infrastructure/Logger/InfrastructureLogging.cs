using Microsoft.Extensions.Logging;

namespace XboxPromotionCheckerBot.App.Infrastructure.Logger;

public static partial class InfrastructureLogging
{
    [LoggerMessage(LogLevel.Information, "Save games to database")]
    public static partial void LogSaveGamesToDatabase(this ILogger logger);
    
    [LoggerMessage(LogLevel.Information, "Games saved to database")]
    public static partial void LogGamesSavedToDatabase(this ILogger logger);
}