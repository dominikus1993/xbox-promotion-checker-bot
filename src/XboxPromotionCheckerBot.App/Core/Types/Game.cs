using System;

namespace XboxPromotionCheckerBot.App.Core.Types;
using Title = string;
using GameId = string;
using Price = decimal;

public readonly struct GamePrice(Price Price, Price? OldPrice);

public sealed class Game
{
    public Game(GameId id, Title title, Uri link, GamePrice gamePrice)
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
}