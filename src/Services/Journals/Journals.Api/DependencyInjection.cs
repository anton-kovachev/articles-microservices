using System.IO.Compression;
using System.Text.Json.Serialization;
using Articles.Security;
using Auth.Grpc;
using Blocks.AspNetCore.Grpc;
using Blocks.AspNetCore.Providers;
using Blocks.Core;
using Blocks.Core.Mapster;
using Blocks.Core.Security;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Json;
using ProtoBuf.Grpc.Server;

namespace Journals.Api;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApiOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAndValiadateOptions<JwtOptions>(configuration)
            .Configure<JsonOptions>(opts =>
            {
                opts.SerializerOptions.PropertyNameCaseInsensitive = true;
                opts.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        return services;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddFastEndpoints()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddJwtAuthentication(configuration)
            .AddMapsterConfigFromCurrentAssembly()
            .AddAuthorization();

        services.AddScoped<IClaimsProvider, HttpContextProvider>()
            .AddScoped<HttpContextAccessor>();

        //server
        services.AddCodeFirstGrpc(options => 
        {
            options.ResponseCompressionLevel = CompressionLevel.Fastest;
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<AssignUserIdInterceptor>();
        });

        var grpcOptions = configuration.GetSectionByTypeName<GrpcServiceOptions>();
        //client
        services.AddCodeFirstGrpcClient<IPersonService>(grpcOptions, "Person");

        return services;
    }
}
