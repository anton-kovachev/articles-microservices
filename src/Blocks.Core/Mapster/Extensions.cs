using Mapster;

namespace Blocks.Core.Mapster;

public static class Extensions
{
    public static TDestination AdaptWith<TDestination>(this object source, Action<TDestination> afterMapping)
    {
        var destination = source.Adapt<TDestination>();
        afterMapping?.Invoke(destination);

        return destination;
     }
}
