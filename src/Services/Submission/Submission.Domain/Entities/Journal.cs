using Domain.Entities;

namespace Submission.Domain.Entities;

public partial class Journal : IEntity<int>
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public required string Abbreviation { get; set; }
    private readonly List<Article> _articles = new();
    public IList<Article> Articles => _articles.AsReadOnly();
}
