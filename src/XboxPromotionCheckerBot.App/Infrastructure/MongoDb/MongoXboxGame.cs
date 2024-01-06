using MongoDB.Bson.Serialization.Attributes;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Infrastructure.MongoDb;

public sealed class MongoXboxGame
{
    public Guid Id { get; set; }
    public long CrawledAtEpoch { get; set; }

    public DateTimeOffset CrawledAt => DateTimeOffset.FromUnixTimeMilliseconds(CrawledAtEpoch);
    
    public MongoXboxGame()
    {
        
    }

    public MongoXboxGame(XboxGame game)
    {
        
    }
}