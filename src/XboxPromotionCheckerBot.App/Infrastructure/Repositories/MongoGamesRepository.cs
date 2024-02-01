using MongoDB.Driver;
using XboxPromotionCheckerBot.App.Core.Repositories;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Infrastructure.MongoDb;

namespace XboxPromotionCheckerBot.App.Infrastructure.Repositories;

public sealed class MongoGamesRepository : IGamesRepository
{
    private readonly IMongoCollection<MongoXboxGame> _games;
    private readonly TimeProvider _timeProvider;
    
    public MongoGamesRepository(IMongoDatabase mongoDatabase, TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        _games = mongoDatabase.Games();
    }

    public async Task<bool> Exists(Game game, CancellationToken cancellationToken = default)
    {
        var filter = Builders<MongoXboxGame>.Filter.Eq(x => x.Id, game.Id);
        return await _games.Find(filter).CountDocumentsAsync(cancellationToken: cancellationToken) > 0;
    }
    
    public async Task Insert(Game game, CancellationToken cancellationToken = default)
    {
        var mongoGame = MapGame(game);
        await _games.InsertOneAsync(mongoGame, cancellationToken: cancellationToken);
    }

    public async Task Insert(IEnumerable<Game> game, CancellationToken cancellationToken = default)
    {
        var xboxGames = MapGames(game);
        await _games.InsertManyAsync(xboxGames, cancellationToken: cancellationToken);
    }
    
    private IEnumerable<MongoXboxGame> MapGames(IEnumerable<Game> games)
    {
        foreach (var game in games)
        {
            yield return MapGame(game);
        }
    }
    
    private MongoXboxGame MapGame(Game game)
    {
        return new MongoXboxGame(game, _timeProvider);
    }
}