using XboxPromotionCheckerBot.App.Infrastructure.Factories;

namespace XboxPromotionCheckerBot.App.Tests.Infrastructure.Factories;

public class FuzzyGameSearcherFactoryTests
{
    [Fact]
    public void TestWhenFileIsCorrect()
    {
        var subject = FuzzyGameSearcherFactory.Produce("./games.csv");

        Assert.NotNull(subject);
    }
    
    [Fact]
    public void TestWhenFileNotExists()
    {
        var subject = Assert.Throws<FileNotFoundException>(() => FuzzyGameSearcherFactory.Produce("./gamesxD.csv"));

        Assert.NotNull(subject);
    }
}