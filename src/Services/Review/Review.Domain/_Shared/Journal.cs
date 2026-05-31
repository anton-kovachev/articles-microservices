using Domain.Entities;

namespace Review.Domain.Articles;
public class Journal : Entity<int>
{
    public required string Name { get; init; }
    public required string Abbreviation { get; init; }
    private readonly List<Article> _articles = new();
    public IReadOnlyList<Article> Articles => _articles.AsReadOnly();
    public IReadOnlyCollection<ReviewerSpecialization> Specializations { get; set; } = new HashSet<ReviewerSpecialization>();
    public void AddArticle(Article article) => _articles.Add(article);
}
