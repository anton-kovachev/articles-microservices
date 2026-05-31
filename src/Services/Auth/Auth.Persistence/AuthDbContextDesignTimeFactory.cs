using Auth.Persistence;
using Blocks.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Persistence;

public sealed class AuthDbContextDesignTimeFactory : DesignTimeFactoryBase<AuthDbContext>
{
    protected override void ConfigureOptions(DbContextOptionsBuilder<AuthDbContext> optionsBuilder, string connectionString)
        => optionsBuilder.UseSqlServer(connectionString);
}
