using ArticleHub.Domain.Articles;
using Articles.Abstractions.Enums;
using Microsoft.VisualBasic;

namespace ArticleHub.Domain.Dtos;

public record ArticleDto
    (
    int Id,
    string Title,
    string Doi,
    ArticleStage Stage,
    DateTime SubmittedOn,
    DateTime? AcceptedOn,
    DateTime? PublishedOn,
    JournalDto Journal,
    PersonDto SubmittedBy);
