
using Journals.Domain.Journal.Enums;
using Redis.OM.Modeling;

namespace Journals.Domain.Journal.ValueObjects;

[Document]
public class SectionEditor
{
    public int EditorId { get; init; }
    public EditorRole Role { get; set; }
}
