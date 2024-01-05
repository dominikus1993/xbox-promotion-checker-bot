using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Notifications;
using XboxPromotionCheckerBot.App.Infrastructure.Filters;
using XboxPromotionCheckerBot.App.Infrastructure.Notifiers;

namespace XboxPromotionCheckerBot.App.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGamesFilter, GameLastSendFilter>();
        services.AddScoped<IGamesFilter, GameNameFilter>();
        services.AddScoped<IGamesFilter, GamePriceFilter>();
        services.AddScoped<IGamesNotifier, DiscordGameNotifier>();
        services.AddScoped<IGamesNotifier, MongoDbGamesNotifier>();
        return services;
    }
}