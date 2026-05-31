using Auth.Persistence;
using EmailService.Smtp;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Articles.Security;
using Auth.Domain.Persons;
using ProtoBuf.Grpc.Server;
using Blocks.Core.Security;

namespace Auth.API;

public static class DependencyConfiguration
{
    public static IServiceCollection ConfigureApiOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAndValiadateOptions<JwtOptions>(configuration);
        return services;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFastEndpoints()
            .SwaggerDocument()
            .AddJwtAuthentication(configuration)
            .AddJwtIdentity(configuration)
            .AddAuthorization();

        services.AddSmtpMailService(configuration);

        services.AddSingleton<GrpcTypeAdapterConfig>();
        services.AddCodeFirstGrpc(options =>
        {
            options.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Fastest;
            options.EnableDetailedErrors = true;
        });
        return services;
    }

    public static IServiceCollection AddJwtIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<User>(options =>
        {
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
        })
        .AddRoles<Domain.Roles.Role>()
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddSignInManager<SignInManager<User>>()
        .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
        });

        return services;
    }
}
