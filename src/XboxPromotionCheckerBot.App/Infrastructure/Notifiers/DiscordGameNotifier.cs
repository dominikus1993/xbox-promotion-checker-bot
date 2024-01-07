using Discord;
using Discord.Webhook;
using XboxPromotionCheckerBot.App.Core.Notifications;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Infrastructure.Notifiers;

public sealed class DiscordGameNotifier : IGamesNotifier
{
    private readonly DiscordWebhookClient _discordWebhookClient;

    public DiscordGameNotifier(DiscordWebhookClient discordWebhookClient)
    {
        _discordWebhookClient = discordWebhookClient;
    }

    public async Task Notify(IReadOnlyList<XboxGame> games, CancellationToken cancellationToken = default)
    {
        var embeds = MapEmbeds(games).Chunk(10);
        var tasks = new List<Task>();
        
        foreach (var messages in embeds)
        {
            tasks.Add(_discordWebhookClient.SendMessageAsync("Witam serdecznie, oto nowe gry w promocji", false, messages));   
        }

        await Task.WhenAll(tasks);
    }

    private IEnumerable<Embed> MapEmbeds(IEnumerable<XboxGame> games)
    {
        foreach (var game in games)
        {
            var description = $"Witam gra potaniala z {game.GamePrice.OldPrice} do {game.GamePrice.Price} co daje promke {game.PromotionPercentage} procent";
            var builder = new EmbedBuilder().WithColor(GetColorByPromotionLevel(game)).WithUrl(game.Link.ToString())
                .WithTitle(game.Title).WithDescription(description);
            yield return builder.Build();
        }
    }

    private static Color GetColorByPromotionLevel(XboxGame game) => game.PromotionPercentage.Value switch
    {
        > 90 => Color.Gold,
        > 70 => Color.Red,
        > 50 => Color.Green,
        _ => Color.Default,
    };
}