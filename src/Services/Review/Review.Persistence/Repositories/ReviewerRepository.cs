using Review.Domain.Reviewers;

namespace Review.Persistence.Repositories
{
    public class ReviewerRepository(ReviewDbContext dbContext) : Repository<Reviewer>(dbContext)
    {
        public async Task<Reviewer?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
            => await Entity.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        public async Task<Reviewer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
            => await Entity.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
    }
}
