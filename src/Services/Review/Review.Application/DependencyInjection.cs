
using Blocks.Core.Mapster;
using Blocks.Messaging.MassTransit;
using Blocks.MediatR.Behaviours;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Review.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMapsterConfigFromCurrentAssembly()
                //.AddValidatorsFromAssemblyContaining<InviceReviewerCommandValidator>()
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
}
