using NSubstitute;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Notifications;
using XboxPromotionCheckerBot.App.Core.Providers;
using XboxPromotionCheckerBot.App.Core.Types;
using XboxPromotionCheckerBot.App.Core.UseCases;

namespace XboxPromotionCheckerBot.App.Tests.Core.UseCases;

public sealed class ParseGamesFilterAndSendItUseCaseTests
{
    [Fact]
    public async Task Test()
    {
        // Arrange
        var gamesParser = Substitute.For<IGamesParser>();
        var gamesFilters = Substitute.For<IEnumerable<IGamesFilter>>();
        var gamesBroadcaster = Substitute.For<IGamesBroadcaster>();
        var useCase = new ParseGamesFilterAndSendItUseCase([gamesParser], gamesFilters, gamesBroadcaster);
        
        // Act
        await useCase.Execute();
        
        // Assert
        gamesParser.Received(1).Parse(Arg.Any<CancellationToken>());
        await gamesBroadcaster.Received(1).Broadcast(Arg.Any<IAsyncEnumerable<XboxGame>>(), Arg.Any<CancellationToken>());
    }
}