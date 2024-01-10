using System.Diagnostics;
using System.Runtime.CompilerServices;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using XboxPromotionCheckerBot.App.Core.Providers;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Infrastructure.Logger;

namespace XboxPromotionCheckerBot.App.Infrastructure.Providers;

public sealed class XboxStoreGamesParser : IGamesParser
{
    public const string XboxStoreUrl = "https://www.microsoft.com/pl-pl/store/deals/games/xbox";
    
    public const int Pages = 10;

    private readonly ILogger<XboxStoreGamesParser> _logger;

    public XboxStoreGamesParser(ILogger<XboxStoreGamesParser> logger)
    {
        _logger = logger;
    }

    public async IAsyncEnumerable<XboxGame> Parse([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var pages = Enumerable.Range(0, Pages).Select(page => ParsePage(page + 1, cancellationToken)).ToArray();
        await foreach (var game in AsyncEnumerableEx.Merge(pages).WithCancellation(cancellationToken))
        {
            yield return game;
        }//*[@id="card-1-0"]
    }

    private async IAsyncEnumerable<XboxGame> ParsePage(int page, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = GetPageUrl(page);
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url.ToString(), cancellationToken);
        if (doc is null)
        {
            _logger.LogNoGames(page);
            yield break;
        }
        var cards = doc.DocumentNode.SelectNodes("""//div[contains(@class, 'card-body')]""");
        if (cards is null)
        {
            yield break;
        }

        foreach (var node in cards)
        {
            var game = ParseHtmlNode(node);
            if (game is not null)
            {
                yield return game;
            }
        }
    }


    private static XboxGame? ParseHtmlNode(HtmlNode node)
    {
        var priceNode = node.SelectSingleNode("./p[@aria-hidden='true']");
        if (priceNode is null)
        {
            return null;
        }

        return null;
    }

    private static Uri GetPageUrl(int page)
    {
        if (page == 1)
        {
            return new Uri(XboxStoreUrl);
        }

        var skipItems = (page - 1) * 90;
        return new Uri($"{XboxStoreUrl}?skipitems={skipItems}");
    }
}