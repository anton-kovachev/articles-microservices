using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blocks.EntityFrameworkCore;

public interface IRepository<TEntity> where TEntity : class, IEntity<int>
{
    Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    TEntity Update(TEntity entity);
    void Remove(TEntity entity);
    Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class RepositoryBase<TContext, TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity<int>
    where TContext : DbContext
{
    protected readonly TContext _dbContext;
    protected readonly DbSet<TEntity> _entity;

    public RepositoryBase(TContext dbContext)
    {
        this._dbContext = dbContext;
        this._entity = dbContext.Set<TEntity>();
    }

    public TContext DbContext => this._dbContext;
    public virtual DbSet<TEntity> Entity => this._entity;
    public string TableName => _dbContext.Model.FindEntityType(typeof(TEntity))?.GetTableName() ?? string.Empty;

    protected virtual IQueryable<TEntity> Query => this._entity.AsQueryable();

    public async Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _entity.FindAsync(id, cancellationToken);

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Query.SingleOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        (await _entity.AddAsync(entity, cancellationToken)).Entity;

    public TEntity Update(TEntity entity) => _entity.Update(entity).Entity;

    public async Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var rowsAffected = await _dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM {TableName} where Id = {id}", cancellationToken);
        return rowsAffected > 0;
    }

    public void Remove(TEntity entity) => _entity.Remove(entity);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _dbContext.SaveChangesAsync(ct);
}
