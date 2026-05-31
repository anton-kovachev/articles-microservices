namespace Journals.Api.Features._Shared;

public record JournalDto(int Id, string Abbreviation, string Name, string Description, string ISSN)
{
    public EditorDto Editor { get; set; } = null!;
}
