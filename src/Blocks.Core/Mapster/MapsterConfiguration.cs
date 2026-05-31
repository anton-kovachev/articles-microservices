using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Blocks.Core.Mapster;

public static class MapsterConfiguration
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static IServiceCollection AddMapsterConfigFromCurrentAssembly(this IServiceCollection services, Assembly assembly = null)
    {
        if (assembly is null)
            assembly = Assembly.GetCallingAssembly()!;

        TypeAdapterConfig.GlobalSettings.Scan(assembly);
        return services;
    }
}
