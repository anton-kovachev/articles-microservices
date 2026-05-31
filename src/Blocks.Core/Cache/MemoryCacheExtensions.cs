using Microsoft.Extensions.Caching.Memory;

namespace Blocks.Core.Cache;

public static class MemoryCacheExtensions
{
    public static T GetOrCreateByType<T>(this IMemoryCache memoryCache, Func<ICacheEntry, T> factory) =>
        memoryCache.GetOrCreate<T>(typeof(T).FullName!, factory)!;
}
