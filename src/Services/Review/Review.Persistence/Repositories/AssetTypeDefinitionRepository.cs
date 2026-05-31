using Microsoft.Extensions.Caching.Memory;
using Review.Domain.Assets;

namespace Review.Persistence.Repositories;

public class AssetTypeDefinitionRepository(ReviewDbContext _dbContext, IMemoryCache _cache) : 
    CacheRepository<ReviewDbContext, AssetTypeDefinition, AssetType>(_dbContext, _cache)
{
}
