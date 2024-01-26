using XboxPromotionCheckerBot.App.Core.Crypto;

namespace XboxPromotionCheckerBot.App.Tests.Core.Crypto;

public class IdGeneratorTests
{
    [Fact]
    public void GenerateIdWhenKeysAreEmpty()
    {
        var subject = Assert.Throws<ArgumentOutOfRangeException>(() => IdGenerator.GenerateId());
        Assert.NotNull(subject);
    }
    
    [Fact]
    public void GenerateIdWhenKeysAreNotEmpty()
    {
        var subject = IdGenerator.GenerateId("jp2gmd", "2137");
        Assert.Equal(new Guid("40b674a4-86cb-99e8-2ef7-76ac3e291aca"), subject);
    }
}