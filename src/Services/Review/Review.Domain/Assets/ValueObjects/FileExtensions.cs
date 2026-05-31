using Blocks.Core.Extensions;
using Blocks.Domain.ValueObjects;

namespace Review.Domain.Assets.ValueObjects;

public class FileExtensions : IValueObject
{
    public IReadOnlyList<string> Extensions { get; init; } = null!;

    public bool IsValid(string extension)
        => Extensions.IsEmpty() || Extensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
}
