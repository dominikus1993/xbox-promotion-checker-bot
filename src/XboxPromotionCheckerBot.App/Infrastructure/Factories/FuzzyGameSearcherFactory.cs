using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using XboxPromotionCheckerBot.App.Core.Repositories;
using XboxPromotionCheckerBot.App.Infrastructure.Repositories;

namespace XboxPromotionCheckerBot.App.Infrastructure.Factories;


public static class FuzzyGameSearcherFactory
{
    private sealed class Game
    {
        public string Title { get; set; }
    }
    
    public static FuzzyGameSearcher Produce(string filePath)
    {
        using var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var stream = new StreamReader(file);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };
        using var csv = new CsvReader(stream, config);
        
        var records = csv.GetRecords<Game>();
        
        return FuzzyGameSearcher.Create(records.Select(g => new FuzzGame(g.Title)));
    }
}