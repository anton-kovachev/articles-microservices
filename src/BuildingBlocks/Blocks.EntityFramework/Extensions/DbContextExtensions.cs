using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Blocks.EntityFrameworkCore.Extensions;

public static class DbContextExtensions
{
    public static IHost Migrate<TDbContext>(this IHost host) where TDbContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        dbContext.Database.Migrate();

        return host;
    }
}
