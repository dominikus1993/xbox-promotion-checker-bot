using XboxPromotionCheckerBot.App.Core.Crypto;

namespace XboxPromotionCheckerBot.App.Tests.Core.Crypto;

public class IdGeneratorTests
{
    [Fact]
    public async Task GenerateIdWhenKeysAreEmpty()
    {
        var subject = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => IdGenerator.GenerateId());
        Assert.NotNull(subject);
    }
    
    [Fact]
    public async Task GenerateIdWhenKeysAreNotEmpty()
    {
        var subject = await IdGenerator.GenerateId("jp2gmd", "2137");
        Assert.NotNull(subject);
        Assert.Equal("iJLwXnhXT+uX7V9R4zA5jRlWF9Z7Zuqq9RSkNnANrzo=", subject);
    }
}