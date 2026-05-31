
using Microsoft.EntityFrameworkCore;
using Submission.Domain.Entities;

namespace Submission.Persistence.Repositories;

public class ArticleRepository(SubmissionDbContext dbContext) :
    Repository<Article>(dbContext)
{
    protected override IQueryable<Article> Query => 
        base.Query
        .Include(e => e.Actors).ThenInclude(e => e.Person)
        .Include(e => e.Assets);

    public async Task<Article?> GetFullArticleByIdAsync(int articleId, CancellationToken cancellationToken)
    {
        return await Query
            .Include(e => e.Journal)
            .Include(e => e.SubmittedBy)
            .SingleOrDefaultAsync(e => e.Id == articleId, cancellationToken);
    }
}
