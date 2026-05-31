
using Blocks.Core.Cache;
using Blocks.Domain.Entities;

namespace Submission.Domain.Entities;

public class AssetTypeDefinition : EnumEntity<AssetType>, ICacheable
{
    public required byte MaxFileSizeInMB { get; init; }
    public int MaxFileSizeInBytes => MaxFileSizeInMB * 1024 * 1024;
    public string DefaultFileExtension { get; set; } = default!;
    public required FileExtensions AllowedFileExtensions { get; init; } = default!;
    public int MaxAssetCount { get; init; }
    public bool AllowMultipleAssets => MaxAssetCount > 1;
}
