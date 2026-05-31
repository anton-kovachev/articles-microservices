using Domain.Entities;

namespace Submission.Domain.Entities;

public partial class Person : IEntity<int>
{
    public int Id { get; init; } 
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string? Title { get; set; }
    public required EmailAddress Email { get; init; }
    public required string Affiliation { get; set; }
    public int? UserId { get; init; }
    public string TypeDiscriminator { get; init; } = null!;
    public IReadOnlyList<ArticleActor> Actors { get; private set; } = new List<ArticleActor>();
}
