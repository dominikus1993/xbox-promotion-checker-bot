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

    public int CompareTo(PromotionPercentage other)
    {
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

public sealed class XboxGame
{
    private XboxGame(GameId id, Title title, Uri link, GamePrice gamePrice)
    {
        Id = id;
        Title = title;
        Link = link;
        GamePrice = gamePrice;
    }

    private bool Equals(XboxGame other)
    {
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is XboxGame other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public GameId Id { get; }
    public Title Title { get; }
    public Uri Link { get; }
    public GamePrice GamePrice { get; }

    public PromotionPercentage PromotionPercentage() => GamePrice.CalculatePromotionPercentage();

    public static XboxGame Create(Title title, Uri link, GamePrice gamePrice)
    {
        var id = IdGenerator.GenerateId(title);
        return new XboxGame(id, title, link, gamePrice);
    }
    
    public static XboxGame Create(GameId id, Title title, Uri link, GamePrice gamePrice)
    {
        return new XboxGame(id, title, link, gamePrice);
    }

    public bool IsValidGame()
    {
        return true;
    }
}