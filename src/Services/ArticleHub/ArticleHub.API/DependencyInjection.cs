using Blocks.Core;
using Blocks.Core.Mapster;
using Blocks.Messaging;
using Blocks.Messaging.MassTransit;
using Carter;
using Microsoft.AspNetCore.Http.Json;
using System.Reflection;
using System.Text.Json.Serialization;
using Articles.Security;
using ArticleHub.Persistence;
using Blocks.Core.Security;
using Blocks.AspNetCore.Providers;

namespace ArticleHub.API;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApiOptions(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure and validate options here if needed
        services.AddAndValiadateOptions<RabbitMqOptions>(configuration)
            .AddAndValiadateOptions<HasuraOptions>(configuration)
            .Configure<JsonOptions>(opt =>
            {
                opt.SerializerOptions.PropertyNameCaseInsensitive = true;
                opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter());

            });
        return services;
    }

    public static IServiceCollection AddApiAndApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddCarter()
            .AddHttpContextAccessor()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddJwtAuthentication(configuration)
            .AddAuthorization();

        services.AddScoped<IClaimsProvider, HttpContextProvider>()
            .AddScoped<HttpContextAccessor>();

        services
            .AddMemoryCache()
            .AddMapsterConfigFromCurrentAssembly()
            .AddMassTransitWithRabbitMQ(configuration, Assembly.GetExecutingAssembly());

        return services;
    }
}
