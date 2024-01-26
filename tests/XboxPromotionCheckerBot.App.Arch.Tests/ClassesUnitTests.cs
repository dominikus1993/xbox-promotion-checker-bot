using NetArchTest.Rules;
using XboxPromotionCheckerBot.App.Core.Filters;
using XboxPromotionCheckerBot.App.Core.Notifications;
using XboxPromotionCheckerBot.App.Core.UseCases;
using XboxPromotionCheckerBot.App.Infrastructure.Notifiers;
using XboxPromotionCheckerBot.App.Infrastructure.Providers;
using XboxPromotionCheckerBot.App.Infrastructure.Repositories;

namespace XboxPromotionCheckerBot.App.Arch.Tests;

public class ClassesUnitTests
{
    [Fact]
    public void TestIGamesNotifierShouldBeSealed()
    {
        var result = Types.InAssembly(typeof(ParseGamesFilterAndSendItUseCase).Assembly)
            .That().ImplementInterface(typeof(IGamesNotifier))
            .Should().BeSealed()
            .GetResult();
        
        
        
        Assert.Null(result.FailingTypes);
        Assert.True(result.IsSuccessful);
    }
    
    [Fact]
    public void TestIGamesFilterShouldBeSealed()
    {
        var result = Types.InAssembly(typeof(ParseGamesFilterAndSendItUseCase).Assembly)
            .That().ImplementInterface(typeof(IGamesFilter))
            .Should().BeSealed()
            .GetResult();
        
        Assert.Null(result.FailingTypes);
        Assert.True(result.IsSuccessful);
    }
    
    
    [Fact]
    public void TestTypesInCoreNamespaceShouldNotUseTypesInInfrastructure()
    {
        var result = Types.InAssembly(typeof(ParseGamesFilterAndSendItUseCase).Assembly)
            .That().ResideInNamespaceContaining("Core")
            .ShouldNot().HaveDependencyOnAny(typeof(MongoGamesRepository).Namespace, typeof(XboxStoreGamesParser).Namespace, typeof(DiscordGameNotifier).Namespace)
            .GetResult();
        
        Assert.Null(result.FailingTypes);
        Assert.True(result.IsSuccessful);
    }
}