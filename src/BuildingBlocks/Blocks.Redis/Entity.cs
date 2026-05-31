using Redis.OM.Modeling;

namespace Blocks.Redis;

public abstract class Entity
{
    [RedisIdField]
    [Indexed]
    public int Id { get; set; }
}
