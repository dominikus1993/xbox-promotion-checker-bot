using XboxPromotionCheckerBot.App.Infrastructure.Extensions;

namespace XboxPromotionCheckerBot.App.Tests.Infrastructure.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("abc!", "abc")]
    [InlineData("abc123", "abc123")]
    [InlineData("abc 123", "abc 123")]
    [InlineData("jp2???", "jp2")]
    [InlineData("Train Sim World 4 Compatible Glossop Line Manchester -", "Train Sim World 4 Compatible Glossop Line Manchester")]
    public void RenameSpecialSigns(string input, string expected)
    {
        // Arrange
        // Act
        var result = input.RemoveSpecialSigns();
        
        // Assert
        Assert.Equal(expected, result);
    }
}