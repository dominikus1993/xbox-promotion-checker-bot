using Discord;
using Discord.Webhook;
using Microsoft.Extensions.Logging;
using XboxPromotionCheckerBot.App.Core.Notifications;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Infrastructure.Logger;
using Game = XboxPromotionCheckerBot.App.Core.Types.Game;

namespace XboxPromotionCheckerBot.App.Infrastructure.Notifiers;

public sealed class DiscordGameNotifier : IGamesNotifier
{
    private readonly DiscordWebhookClient _discordWebhookClient;
    private readonly ILogger<DiscordGameNotifier> _logger;

    public DiscordGameNotifier(DiscordWebhookClient discordWebhookClient, ILogger<DiscordGameNotifier> logger)
    {
        _discordWebhookClient = discordWebhookClient;
        _logger = logger;
    }

    public Task Notify(IReadOnlyList<Game> games, CancellationToken cancellationToken = default)
    {
        if (games is {Count: 0})
        {
            _logger.LogNoGamesToSend();
            return Task.CompletedTask;
        }
        
        var embeds = MapEmbeds(games).Chunk(10);
        return SendAll(embeds, _discordWebhookClient, (client, em) => client.SendMessageAsync("Witam serdecznie, oto nowe gry w promocji", false, em));
    }

    private static Task SendAll<T, TDepen>(IEnumerable<T> elements, TDepen dep, Func<TDepen, T, Task> f)
    {
        var tasks = new List<Task>();
        
        foreach (var message in elements)
        {
            tasks.Add(f(dep, message));   
        }

        return Task.WhenAll(tasks);
    }

    private static IEnumerable<Embed> MapEmbeds(IEnumerable<Game> games)
    {
        foreach (var game in games)
        {
            var description = $"Witam gra potaniala z {game.GamePrice.OldPrice} do {game.GamePrice.Price} co daje promke {game.PromotionPercentage} procent";
            var builder = new EmbedBuilder().WithColor(GetColorByPromotionLevel(game)).WithUrl(game.Link.ToString())
                .WithTitle(game.Title).WithDescription(description);
            yield return builder.Build();
        }
    }

    private static Color GetColorByPromotionLevel(Game game) => game.PromotionPercentage() switch
    {
        { Value:> 90 } => Color.Gold,
        { Value:> 70 } => Color.Red,
        { Value:> 50 } => Color.Green,
        _ => Color.Default,
    };
}