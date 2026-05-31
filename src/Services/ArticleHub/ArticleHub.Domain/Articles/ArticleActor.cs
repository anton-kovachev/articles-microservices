using Articles.Abstractions.Enums;

namespace ArticleHub.Domain.Articles;

public class ArticleActor
{
    public required int ArticleId { get; init; }
    public Article Article { get; init; } = null!;
    public required int PersonId { get; init; }
    public Person Person { get; init; } = null!;
    public UserRoleType Role { get; init; }
}
