using System.Runtime.CompilerServices;

namespace XboxPromotionCheckerBot.App.Core.Extensions;

public static class EnumerableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
    {
        ArgumentNullException.ThrowIfNull(enumerable);
        if (enumerable.TryGetNonEnumeratedCount(out var count))
        {
            return count == 0;
        }

        return !enumerable.Any();
    }
}