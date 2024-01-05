using System;
using XboxPromotionCheckerBot.App.Core.Crypto;

namespace XboxPromotionCheckerBot.App.Core.Types;
using Title = string;
using GameId = string;
using Price = decimal;

public readonly struct GamePrice(Price Price, Price? OldPrice);

public sealed class Game
{
    private Game(GameId id, Title title, Uri link, GamePrice gamePrice)
    {
        Id = id;
        Title = title;
        Link = link;
        GamePrice = gamePrice;
    }

    public GameId Id { get; }
    public Title Title { get; }
    public Uri Link { get; }
    public GamePrice GamePrice { get; }


    public static async Task<Game> Create(Title title, Uri link, GamePrice gamePrice)
    {
        var id = await IdGenerator.GenerateId(title);
        return new Game(id, title, link, gamePrice);
    }
    
    public static Game Create(GameId id, Title title, Uri link, GamePrice gamePrice)
    {
        return new Game(id, title, link, gamePrice);
    }
}