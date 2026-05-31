using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;

public static class DependencyConfiguration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<TokenFactory>();
        return services;
    }
}
