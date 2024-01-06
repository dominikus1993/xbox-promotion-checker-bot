using MongoDB.Driver;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Infrastructure.MongoDb;

namespace XboxPromotionCheckerBot.App.Infrastructure.Repositories;

public sealed class MongoGamesRepository
{
    private readonly IMongoCollection<MongoXboxGame> _games;
    
    public MongoGamesRepository(IMongoDatabase mongoDatabase)
    {
        _games = mongoDatabase.Games();
    }

    public async Task<bool> Exists(XboxGame game, CancellationToken cancellationToken = default)
    {
        var filter = Builders<MongoXboxGame>.Filter.Eq(x => x.Id, game.Id);
        return await _games.Find(filter).CountDocumentsAsync(cancellationToken: cancellationToken) > 0;
    }
    
    public async Task Insert(XboxGame game, CancellationToken cancellationToken = default)
    {
        var mongoGame = new MongoXboxGame(game);
        await _games.InsertOneAsync(mongoGame, cancellationToken: cancellationToken);
    }
}