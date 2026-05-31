using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Blocks.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddContreteImplementationOfGeneric(
        this IServiceCollection services,
        Type genericBaseType,
        Assembly[]? assemblies = null,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        assemblies ??= new[] { Assembly.GetCallingAssembly() };

        var implementations = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && InheritsFrom(t, genericBaseType));

        foreach (var implmentation in implementations)
        {
            services.Add(new ServiceDescriptor(implmentation, implmentation, serviceLifetime));
        }
        return services;
    }

    private static bool InheritsFrom(Type type, Type baseType)
    {
        var current = type;
        while (current != null && current != typeof(object))
        {
            // Check for exact match (non-generic inheritance)
            if (current == baseType)
                return true;

            // Check for generic type inheritance
            if (current.IsGenericType && baseType.IsGenericTypeDefinition &&
                current.GetGenericTypeDefinition() == baseType)
                return true;

            current = current.BaseType;
        }
        return false;
    }
}
