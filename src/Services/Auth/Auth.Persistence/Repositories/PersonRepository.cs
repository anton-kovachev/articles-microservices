using Auth.Domain.Persons;
using Blocks.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Persistence.Repositories;

public class PersonRepository(AuthDbContext dbContext) : RepositoryBase<AuthDbContext, Person>(dbContext)
{
    protected override IQueryable<Person> Query => base.Query.Include(q => q.User);

    public async Task<Person?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default) =>
        await Query.SingleOrDefaultAsync(p => p.UserId == userId, cancellationToken);

    public async Task<Person?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await Query.SingleOrDefaultAsync(p => p.Email.NormalizedEmail == email.ToUpperInvariant(), cancellationToken);
}
