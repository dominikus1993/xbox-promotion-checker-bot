using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using XboxPromotionCheckerBot.App.Core.Providers;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Infrastructure.Logger;

namespace XboxPromotionCheckerBot.App.Infrastructure.Providers;

public sealed partial class XboxStoreGamesParser : IGamesParser
{
    [GeneratedRegex(@"(\d+,\d{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase, 1000)]
    private static partial Regex PriceRegex();
    private const string XboxStoreUrl = "https://www.microsoft.com/pl-pl/store/deals/games/xbox";
    
    private const int Pages = 10;

    private readonly ILogger<XboxStoreGamesParser> _logger;

    public XboxStoreGamesParser(ILogger<XboxStoreGamesParser> logger)
    {
        _logger = logger;
    }

    public IAsyncEnumerable<XboxGame> Parse(CancellationToken cancellationToken = default)
    {
        var pages = ParsePages(Pages, cancellationToken).ToArray();
        return AsyncEnumerableEx.Merge(pages);
    }
    
    private IEnumerable<IAsyncEnumerable<XboxGame>> ParsePages(int pages, CancellationToken cancellationToken = default)
    {
        for (int page = 1; page <= pages; page++)
        {
            yield return ParsePage(page, cancellationToken);
        }
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


    private XboxGame? ParseHtmlNode(HtmlNode node)
    {
        
        var titleAndUrl = ParseTitleAndLink(node);
        if (!titleAndUrl.HasValue)
        {
            return null;
        }
        
        var price = ParsePrice(node);
        if (price is null)
        {
            return null;
        }

        var (title, link) = titleAndUrl.Value;
        var result = XboxGame.Create(title, link, price.Value);
        
        if (result.IsValidGame())
        {
            return result;
        }
        
        return null;
    }

    private GamePrice? ParsePrice(HtmlNode node)
    {
        var priceNode = node.SelectSingleNode("./p[@aria-hidden='true']");
        if (priceNode is null)
        {
            return null;
        }

        var pricesText = priceNode.InnerText;

        var elements = PriceRegex().Matches(pricesText);

        if (elements is [var oldPrice, var promotionalPrice])
        {
            var price = promotionalPrice.Value.Replace(",", ".", StringComparison.InvariantCultureIgnoreCase);
            var oldP = oldPrice.Value.Replace(",", ".", StringComparison.InvariantCultureIgnoreCase);
            return new GamePrice(decimal.Parse(price, CultureInfo.InvariantCulture),
                decimal.Parse(oldP, CultureInfo.InvariantCulture));
        }
        
        _logger.LogCantFindPrices(priceNode.InnerText);
        return null;
    }

    private (string Title, Uri Link)? ParseTitleAndLink(HtmlNode node)
    {
        var titleNode = node.SelectSingleNode("./h3/a");
        if (titleNode is null)
        {
            return null;
        }

        var link = titleNode.Attributes["href"];
        if (!string.IsNullOrEmpty(link.Value) && Uri.TryCreate(link.Value, UriKind.Absolute, out var uri))
        {
            var title = titleNode.InnerText!;
        
            return (title, uri);
        }
        
        _logger.LogCantParseUrl(link.Value);
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