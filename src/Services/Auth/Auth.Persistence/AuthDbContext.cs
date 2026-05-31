using Auth.Domain.Roles;
using Auth.Domain.Users;
using Auth.Domain.Persons;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Persistence;

public class AuthDbContext(DbContextOptions<AuthDbContext> dbContextOptions) : IdentityDbContext<User, Role, int>(dbContextOptions)
{
    public virtual DbSet<Person> Persons { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
