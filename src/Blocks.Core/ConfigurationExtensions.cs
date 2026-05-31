
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blocks.Core;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddAndValiadateOptions<TOptions>(this IServiceCollection services, IConfiguration configuration) where TOptions : class
    {
        var section = configuration.GetSection(typeof(TOptions).Name);

        if (!section.Exists())
            throw new InvalidOperationException($"Configuration section '{section.Key}' is missing.");

        services.AddOptions<TOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    public static T GetSectionByTypeName<T>(this IConfiguration configuration)
    {
        var sectionName = typeof(T).Name;
        var section = configuration.GetSection(sectionName).Get<T>();

        return Guard.AgainstNulll(section, sectionName);
    }

    public static string GetConnectionStringOrThrow(this IConfiguration configuration, string name)
    {
        var connectionString = configuration.GetConnectionString(name);

        if(string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException($"Connection string '{name}' is missing.");

        return connectionString!;
    }
}
