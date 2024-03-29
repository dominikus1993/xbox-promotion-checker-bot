using MongoDB.Bson.Serialization.Attributes;
using XboxPromotionCheckerBot.App.Core.Types;

namespace XboxPromotionCheckerBot.App.Infrastructure.MongoDb;

public sealed class MongoXboxGame
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Uri Link { get; set; }
    public GamePrice GamePrice { get; set; }
    public string Platform { get; set; }
    public long CrawledAtEpoch { get; set; }

    public DateTimeOffset CrawledAt => DateTimeOffset.FromUnixTimeMilliseconds(CrawledAtEpoch);
    
    public MongoXboxGame()
    {
        
    }

    public MongoXboxGame(Game game, TimeProvider provider)
    {
        Id = game.Id;
        Title = game.Title;
        Link = game.Link;
        GamePrice = game.GamePrice;
        CrawledAtEpoch = provider.GetUtcNow().ToUnixTimeMilliseconds();
        Platform = game.Platform;
    }
}