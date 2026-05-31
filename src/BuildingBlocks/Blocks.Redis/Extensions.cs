
using Blocks.Exceptions;
using Redis.OM.Searching;

namespace Blocks.Redis;
public static class Extensions

{
    public static async Task<T> GetByIdOrThrowAsync<T>(this IRedisCollection<T> collection, int id)
    {
        var entity = await collection.FindByIdAsync(id.ToString());
        if (entity == null)
        {
            throw new NotFoundException($"Item with ID '{id}' not found.");
        }
        return entity;
    }
}