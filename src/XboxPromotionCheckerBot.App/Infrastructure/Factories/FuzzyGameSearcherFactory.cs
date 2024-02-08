using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using nietras.SeparatedValues;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Repositories;
using XboxPromotionCheckerBot.App.Infrastructure.Filters;
using XboxPromotionCheckerBot.App.Infrastructure.Repositories;

namespace XboxPromotionCheckerBot.App.Infrastructure.Factories;


public static class FuzzyGameSearcherFactory
{
    public static GameNameFilter Produce(string filePath)
    {
        using var reader = Sep.Reader().FromFile(filePath);

        var res = new List<FuzzGame>();
        foreach (var row in reader)
        {
            var title = row["Title"].Parse<string>();
            res.Add(new FuzzGame(title));
        }
        
        return new GameNameFilter(res);
    }
}