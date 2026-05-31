using Blocks.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Submission.Persistence;

public sealed class SubmissionDbContextDesignTimeFactory : DesignTimeFactoryBase<SubmissionDbContext>
{
    protected override void ConfigureOptions(DbContextOptionsBuilder<SubmissionDbContext> optionsBuilder, string connectionString)
        => optionsBuilder.UseSqlServer(connectionString);
}
