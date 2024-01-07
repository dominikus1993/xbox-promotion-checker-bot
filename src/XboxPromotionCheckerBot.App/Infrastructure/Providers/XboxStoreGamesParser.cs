using System.Diagnostics;
using System.Runtime.CompilerServices;
using HtmlAgilityPack;
using XboxPromotionCheckerBot.App.Core.Providers;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Infrastructure.Providers;

public sealed class XboxStoreGamesParser : IGamesParser
{
    public const string XboxStoreUrl = "https://www.microsoft.com/pl-pl/store/deals/games/xbox";
    
    public const int Pages = 10;
    public async IAsyncEnumerable<XboxGame> Parse([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var pages = Enumerable.Range(0, Pages).Select(page => ParsePage(page + 1, cancellationToken)).ToArray();
        await foreach (var game in AsyncEnumerableEx.Merge(pages).WithCancellation(cancellationToken))
        {
            yield return game;
        }
    }

    private async IAsyncEnumerable<XboxGame> ParsePage(int page, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = GetPageUrl(page);
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url.ToString(), cancellationToken);
        Console.WriteLine("XD");
        yield break;
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