using Review.Domain.Invitations;

namespace Review.Persistence.Repositories
{
    public class ReviewInvitationRepository(ReviewDbContext _dbContext) : Repository<ReviewInvitation>(_dbContext)
    {
        public async Task<ReviewInvitation> GetByTokenOrThrowAsync(string token, CancellationToken cancellationToken = default)
            => await Entity.SingleOrThrowAsync(i => i.Token.Value == token, cancellationToken);
    }
}
