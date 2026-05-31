
using Blocks.Domain.ValueObjects;

namespace Review.Domain.Assets.ValueObjects;

public class FileExtension : StringValueObject
{
    internal FileExtension(string value) => Value = value;

    public static FileExtension FromFileName(string fileName, AssetTypeDefinition assetTypeDefinition)
    {
        var extension = Path.GetExtension(fileName)?.TrimStart('.').ToLower();
        ArgumentException.ThrowIfNullOrWhiteSpace(extension);
        ArgumentOutOfRangeException.ThrowIfNotEqual(assetTypeDefinition.AllowedFileExtensions.IsValid(extension), true);

        return new FileExtension(extension);
    }
}
