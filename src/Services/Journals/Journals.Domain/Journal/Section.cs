using Blocks.Redis;
using Journals.Domain.Journal.ValueObjects;
using Redis.OM.Modeling;

namespace Journals.Domain.Journal;

[Document(StorageType = StorageType.Json)]
public class Section : Entity
{
    [Indexed]
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<SectionEditor> Editors { get; set; } = new();
}
