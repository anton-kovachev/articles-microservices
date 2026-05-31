using Blocks.Core.Mapster;
using Blocks.MediatR.Behaviours;
using Blocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Submission.Application.Features.CreateArticle;
using System.Reflection;

namespace Submission.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMapsterConfigFromCurrentAssembly()
            .AddValidatorsFromAssemblyContaining<CreateArticleCommandValidator>()
            .AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(AssignUserIdBehavior<,>));
            })
            .AddMassTransitWithRabbitMQ(configuration, Assembly.GetExecutingAssembly());

        return services;
    }
}
