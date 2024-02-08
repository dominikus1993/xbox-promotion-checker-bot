using Discord.Webhook;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Notifications;
using XboxPromotionCheckerBot.App.Core.Providers;
using XboxPromotionCheckerBot.App.Core.Repositories;
using XboxPromotionCheckerBot.App.Infrastructure.Factories;
using XboxPromotionCheckerBot.App.Infrastructure.MongoDb;
using XboxPromotionCheckerBot.App.Infrastructure.Notifiers;
using XboxPromotionCheckerBot.App.Infrastructure.Providers;
using XboxPromotionCheckerBot.App.Infrastructure.Repositories;

namespace XboxPromotionCheckerBot.App.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    private static DiscordWebhookClient CreateDiscordWebhookClient(string webhookurl)
    {
        ArgumentException.ThrowIfNullOrEmpty(webhookurl);
        return new DiscordWebhookClient(webhookurl);
    }
    
    public static async Task<IServiceCollection> AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var client = MongoDbSetup.MongoClient(configuration.GetConnectionString("Games")!);
        var db = client.GamesDb();
        await db.Setup();

        services.AddSingleton<TimeProvider>(TimeProvider.System);
        services.AddSingleton<IMongoClient>(client);
        services.AddSingleton<IMongoDatabase>(db);

        services.AddSingleton<DiscordWebhookClient>(sp =>
        {
            var cfg = sp.GetRequiredService<IConfiguration>();
            var url = cfg.GetConnectionString("DiscordWebhookUrl");
            return CreateDiscordWebhookClient(url);
        });

        services.AddSingleton<IGamesFilter, GameNameFilter>(sp =>
        {
            var cfg = sp.GetRequiredService<IConfiguration>();
            return FuzzyGameSearcherFactory.Produce(cfg.GetConnectionString("FuzzyGamesFilePath")!);
        });
        services.AddSingleton<GameNameFilter>(sp =>
        {
            var cfg = sp.GetRequiredService<IConfiguration>();
            return FuzzyGameSearcherFactory.Produce(cfg.GetConnectionString("FuzzyGamesFilePath")!);
        });
        services.AddScoped<IGamesRepository, MongoGamesRepository>();
        services.AddScoped<IGamesFilter, GamePriceFilter>();
        services.AddScoped<IGamesFilter, GameLastSendFilter>();
        services.AddScoped<IGamesNotifier, DiscordGameNotifier>();
        services.AddScoped<IGamesNotifier, MongoDbGamesNotifier>();
        services.AddScoped<IGamesParser, XboxStoreGamesParser>();
        services.AddHttpClient<IGamesParser, SteamGamesParser>(client =>
        {
            client.DefaultRequestHeaders.UserAgent.Add(new("Safari", "605.1.15"));
        });
        return services;
    }
}