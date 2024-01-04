using System;

namespace XboxPromotionCheckerBot.App.Core.Types;
using Title = string;
using GameId = string;
public sealed class Game
{
    public GameId Id { get; }
    public Title Title { get; }
    public Uri Link { get; }
    
}