using System;
using XboxPromotionCheckerBot.App.Core.Crypto;

namespace XboxPromotionCheckerBot.App.Core.Types;
using Title = string;
using GameId = Guid;
using Price = decimal;

public sealed record PromotionPercentage(double Value) : IComparable<PromotionPercentage>
{
    public override string ToString()
    {
        return $"{Value:F}";
    }
    
    public static readonly PromotionPercentage Zero = new PromotionPercentage(0d);

    public int CompareTo(PromotionPercentage? other)
    {
        if (other is null)
        {
            return -1;
        }
        return Value.CompareTo(other.Value);
    }

    public static bool operator <=(PromotionPercentage left, PromotionPercentage right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(PromotionPercentage left, PromotionPercentage right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static bool operator <(PromotionPercentage left, PromotionPercentage right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(PromotionPercentage left, PromotionPercentage right)
    {
        return left.CompareTo(right) > 0;
    }
}

public readonly record struct GamePrice(Price Price, Price? OldPrice = default)
{
    public PromotionPercentage CalculatePromotionPercentage()
    {
        
        if (!OldPrice.HasValue)
        {
            return PromotionPercentage.Zero;
        }

        if (Price >= OldPrice.Value)
        {
            return PromotionPercentage.Zero;
        }

        var res =  100d - (Price.ToDouble(Price) / Price.ToDouble(OldPrice.Value) * 100d);
        return new PromotionPercentage(Math.Round(res, 2, MidpointRounding.AwayFromZero));
    }
}

public sealed class Game
{
    private Game(GameId id, Title title, Uri link, GamePrice gamePrice, string platform)
    {
        Id = id;
        Title = title;
        Link = link;
        GamePrice = gamePrice;
        Platform = platform;
    }

    private bool Equals(Game other)
    {
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Game other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public GameId Id { get; }
    public Title Title { get; }
    public string Platform { get; }
    public Uri Link { get; }
    public GamePrice GamePrice { get; }

    public PromotionPercentage PromotionPercentage() => GamePrice.CalculatePromotionPercentage();

    public static Game Create(Title title, Uri link, GamePrice gamePrice, string platform)
    {
        var id = IdGenerator.GenerateId(title);
        return new Game(id, title, link, gamePrice, platform);
    }
    
    public static Game Create(GameId id, Title title, Uri link, GamePrice gamePrice, string platform)
    {
        return new Game(id, title, link, gamePrice, platform);
    }

    public bool IsValidGame()
    {
        if (GamePrice is {Price: > 0})
        {
            return true;
        }

        return false;
    }
}