using Domain.Entities;

namespace Submission.Persistence.Repositories;

public class Repository<TEntity>(SubmissionDbContext dbContext) : Blocks.EntityFrameworkCore.RepositoryBase<SubmissionDbContext, TEntity>(dbContext)
    where TEntity : class, IEntity<int>
{ 
}