using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Blocks.EntityFrameworkCore;
public static class BuilderExtensions 
{
    public static PropertyBuilder<TEnum> HasEnumConversion<TEnum>(this PropertyBuilder<TEnum> builder) where TEnum : Enum
    {
        return builder.HasConversion(
            x => x.ToString(),
            x => (TEnum)Enum.Parse(typeof(TEnum), x)
        );
    }

    public static PropertyBuilder<T> HasJsonCollectionConversion<T>(this PropertyBuilder<T> builder)
    {
        return builder.HasConversion(BuildJsonListConvertor<T>());
    }

    public static ValueConverter<TCollection, string> BuildJsonListConvertor<TCollection>()
    {
        Func<TCollection, string> serializeFunc = collection => JsonSerializer.Serialize(collection);
        Func<string, TCollection> deserializeFunc = json => JsonSerializer.Deserialize<TCollection>(json ?? "[]");

        return new ValueConverter<TCollection, string>(
            v => serializeFunc(v),
            v => deserializeFunc(v));
    }

    public static ValueConverter<IReadOnlyList<T>, string> BuildJsonReadOnlyListConverter<T>()
    {
        Func<IReadOnlyList<T>, string> serializeFunc = collection => JsonSerializer.Serialize(collection);
        Func<string, IReadOnlyList<T>> deserializeFunc = json => JsonSerializer.Deserialize<IReadOnlyList<T>>(json ?? "[]");

        return new ValueConverter<IReadOnlyList<T>, string>(
            v => serializeFunc(v),
            v => deserializeFunc(v));
    }

    public static PropertyBuilder<T> HasColumnNameAsProperty<T>(this PropertyBuilder<T> builder)
        => builder.HasColumnName(builder.Metadata.PropertyInfo!.Name);
}
