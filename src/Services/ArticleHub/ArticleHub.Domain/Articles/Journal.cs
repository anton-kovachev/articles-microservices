
using Domain.Entities;

namespace ArticleHub.Domain.Articles;

public class Journal : Entity<int>
{
    public required string Name { get; init; } 
    public required string Abbreviation { get; init; }
    public ICollection<Article> Articles { get; } = new List<Article>();
}
