using Articles.Abstractions.Enums;

namespace Articles.IntregationEvents.Contracts.Articles.Dtos;

public record ArticleDto(
    int Id,
    string Title,
    string Scope,
    string? Doi,
    ArticleType Type,
    ArticleStage Stage,
    PersonDto SubmittedBy,
    DateTime SubmittedOn,
    JournalDto Journal,
    List<ActorDto> Actors,
    List<AssetDto> Assets);
