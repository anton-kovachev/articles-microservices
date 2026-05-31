using Blocks.Core.Cache;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blocks.EntityFrameworkCore;
public class CacheRepository<TDbContext, TEntity, TId>(TDbContext _dbContext, IMemoryCache _cache)
    where TDbContext : DbContext
    where TEntity : class, IEntity<TId>, ICacheable
    where TId: struct
{
    public IEnumerable<TEntity> GetAll() =>
        _cache.GetOrCreateByType<IEnumerable<TEntity>>(entry => _dbContext.Set<TEntity>().AsNoTracking().ToList())!;

    public TEntity GetById(TId id) => GetAll().Single(e => e.Id.Equals(id))!;
}
