
using Blocks.Domain.ValueObjects;

namespace Review.Domain.Assets.ValueObjects;

public partial class File : IValueObject
{
    public required string OriginalName { get; init; } = default!;
    public required string FileServerId { get; init; } = default!;
    public required long Size { get; init; }
    public required FileName Name { get; init; }
    public required FileExtension Extension { get;  init;} = default!;
}
