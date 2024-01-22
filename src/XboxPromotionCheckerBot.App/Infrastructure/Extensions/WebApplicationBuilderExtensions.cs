using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Notifications;
using XboxPromotionCheckerBot.App.Core.Providers;
using XboxPromotionCheckerBot.App.Infrastructure.MongoDb;
using XboxPromotionCheckerBot.App.Infrastructure.Notifiers;
using XboxPromotionCheckerBot.App.Infrastructure.Providers;

namespace XboxPromotionCheckerBot.App.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static async Task<IServiceCollection> AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var client = MongoDbSetup.MongoClient(configuration.GetConnectionString("Games"));
        var db = client.GamesDb();
        await db.Setup();
        services.AddSingleton<IMongoClient>(client);
        services.AddSingleton<IMongoDatabase>(db);
        
        services.AddScoped<IGamesFilter, GamePriceFilter>();
        services.AddScoped<IGamesFilter, GameNameFilter>();
        services.AddScoped<IGamesFilter, GameLastSendFilter>();
        
        services.AddScoped<IGamesNotifier, DiscordGameNotifier>();
        services.AddScoped<IGamesNotifier, MongoDbGamesNotifier>();
        services.AddScoped<IGamesParser, XboxStoreGamesParser>();
        return services;
    }
}