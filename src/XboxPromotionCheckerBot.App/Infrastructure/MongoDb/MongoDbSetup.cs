using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using XboxPromotionCheckerBot.App.Core.Types;
using static MongoDB.Driver.Builders<XboxPromotionCheckerBot.App.Infrastructure.MongoDb.MongoXboxGame>;

namespace XboxPromotionCheckerBot.App.Infrastructure.MongoDb;

public static class MongoDbSetup
{
    public static IMongoCollection<MongoXboxGame> Games(this IMongoDatabase db) => db.GetCollection<MongoXboxGame>("games");

    public static IMongoDatabase GamesDb(this IMongoClient client) => client.GetDatabase("Games");
    
    
    
    public static async Task Setup(this IMongoDatabase db)
    {
        BsonClassMap.RegisterClassMap<MongoXboxGame>(classMap =>
        {
            classMap.AutoMap();
            classMap.UnmapField(x => x.CrawledAt);
            classMap.SetIgnoreExtraElements(true);
            classMap.MapIdField(a => a.Id);
        });
        var games = db.Games();
        await games.Indexes.CreateManyAsync([
            new CreateIndexModel<MongoXboxGame>(IndexKeys.Ascending(x => x.CrawledAtEpoch),
                new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(7) })
        ]);
    }
}