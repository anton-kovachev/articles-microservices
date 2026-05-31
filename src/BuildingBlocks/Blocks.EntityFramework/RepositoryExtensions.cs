
using Blocks.Core;
using Blocks.Exceptions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace Blocks.EntityFrameworkCore;

public static class RepositoryExtensions
{
    public static async Task<TEntity> FindByIdOrThrowAsync<TContext, TEntity>(this RepositoryBase<TContext, TEntity> repository, int id)
        where TEntity : class, IEntity<int>
        where TContext : DbContext

    {
        TEntity? entity = await repository.FindByIdAsync(id);

        if (entity is null)
            throw new NotFoundException($"Entity of type {typeof(TEntity).Name} with id {id} not found");


        return entity;
    }
    public static async Task<TEntity> FindByIdOrThrowAsync<TEntity>(this DbSet<TEntity> dbSet, int id)
        where TEntity : class, IEntity<int>
    {
        TEntity? entity = await dbSet.FindAsync(id);

        if (entity is null)
            throw new NotFoundException($"Entity of type {typeof(TEntity).Name} with id {id} not found");


        return entity;
    }

    public static async Task<TEntity> GetByIdOrThrowAsync<TContext, TEntity>(this RepositoryBase<TContext, TEntity> repository, int id, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity<int>
        where TContext : DbContext

    {
        TEntity? entity = await repository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            throw new NotFoundException($"Entity of type {typeof(TEntity).Name} with id {id} not found");


        return entity;
    }

    public static async Task<TEntity> SingleOrThrowAsync<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        where TEntity : class, IEntity<int>
         => Guard.NotFound(await source.SingleOrDefaultAsync(predicate, cancellationToken));
}
