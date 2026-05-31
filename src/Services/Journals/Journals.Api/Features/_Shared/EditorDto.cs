using Journals.Domain.Journal.Enums;

namespace Journals.Api.Features._Shared;

public record EditorDto(int Id, string FullName, string Affiliation, EditorRole Role = EditorRole.ChiefEditor);
