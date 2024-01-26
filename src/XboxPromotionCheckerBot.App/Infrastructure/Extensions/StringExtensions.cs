using System.Text.RegularExpressions;

namespace XboxPromotionCheckerBot.App.Infrastructure.Extensions;

public static partial class StringExtensions
{
    
    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex SpecialSignRegex();
    
    public static string RemoveSpecialSigns(this string txt)
    {
        var regex = SpecialSignRegex();
        return regex.Replace(txt, string.Empty);
    }
}