
namespace Blocks.Core.Extensions;

public static class EnumerableExtensions
{
    public static bool IsEmpty<T>(this IEnumerable<T> enumerable) => !enumerable.Any();
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable is null || !enumerable.Any();
    public static bool IsNotNullEmpty<T>(this IEnumerable<T> enumerable) => enumerable is not null && enumerable.Any();
}
