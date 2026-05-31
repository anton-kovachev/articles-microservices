using Redis.OM;
using Redis.OM.Searching;
using StackExchange.Redis;
using Journals.Domain.Journal;

namespace Journals.Persistence;

public class JournalDbContext
{
    private readonly IDatabase _redisDb;
    private readonly RedisConnectionProvider _provider;

    public JournalDbContext(IConnectionMultiplexer redis, RedisConnectionProvider provider)
        => (_redisDb, _provider) = (redis.GetDatabase(), provider);

    public IRedisCollection<Journal> Journals => _provider.RedisCollection<Journal>();
    public IRedisCollection<Editor> Editors => _provider.RedisCollection<Editor>();

    RedisConnectionProvider Provider => _provider;
}
