using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XboxPromotionCheckerBot.App.Core.Notifications;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Core.UseCases;

namespace XboxPromotionCheckerBot.App.Core.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<PromotionPercentage>(new PromotionPercentage(40));
        services.AddScoped<IGamesBroadcaster, DefaultGamesBroadcaster>();
        services.AddScoped<ParseGamesFilterAndSendItUseCase>();
        return services;
    }
}