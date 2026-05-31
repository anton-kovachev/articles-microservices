using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Blocks.EntityFrameworkCore;

public abstract class DesignTimeFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext>
    where TContext : DbContext
{
    public TContext CreateDbContext(string[] args)
    {
        var configuration = Host.CreateApplicationBuilder(args).Configuration;
        var connectionString = configuration.GetConnectionString("Database") ??
            throw new InvalidOperationException("Missing: 'ConnectionString:Database'");

        var optionsBuilder = new DbContextOptionsBuilder<TContext>();
        ConfigureOptions(optionsBuilder, connectionString);

        var dbContext = Activator.CreateInstance(typeof(TContext),optionsBuilder.Options) ??
            throw new InvalidOperationException($"{typeof(TContext).Name} needs a constructor ctor(DbContextOptions<{typeof(TContext).Name}>)");

        return dbContext as TContext;
    }

    protected abstract void ConfigureOptions(DbContextOptionsBuilder<TContext> optionsBuilder, string connectionString);
}
