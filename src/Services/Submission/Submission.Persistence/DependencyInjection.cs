using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Blocks.EntityFrameworkCore;
using Blocks.EntityFrameworkCore.Interceptors;
using Articles.Abstractions.Enums;
using Submission.Domain.Entities;
using Submission.Persistence.Repositories;
using System.Reflection;
using Blocks.MediatR.Behaviours;

namespace Submission.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        services.AddMediatR(config =>
          {
              config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
              config.AddOpenBehavior(typeof(ValidationBehavior<,>));
              config.AddOpenBehavior(typeof(AssignUserIdBehavior<,>));
          });
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddDbContext<SubmissionDbContext>((provider, options) => 
        {
            options.AddInterceptors(provider.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure();
            });
        });

        services.AddScoped(typeof(Repository<>));
        services.AddScoped(typeof(ArticleRepository));
        services.AddScoped(typeof(JournalRepository));
        services.AddScoped(typeof(PersonRepository));
        services.AddScoped(typeof(ArticleStateMachineFactory));
        services.AddScoped<CacheRepository<SubmissionDbContext, AssetTypeDefinition, AssetType>>();

        return services;
    }
}
