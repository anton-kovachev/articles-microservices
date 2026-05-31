using Blocks.Domain.Entities;

namespace Submission.Domain.Entities;

public partial class Article : AggregateRoot 
{
    public int Id { get; init; }
    public required string Title { get; set; }
    public required string Scope { get; set; }
    public required ArticleType Type { get; set; }
    public ArticleStage Stage { get; internal set; }
    public int JournalId { get; init; }
    public Journal Journal { get; init; } = null!;
    public int? SubmittedById { get; set; }
    public Person? SubmittedBy { get; set; } = null!;
    public DateTime? SubmittedOn { get; internal set; }

    private readonly List<Asset> _assets = new();
    public IReadOnlyList<Asset> Assets => _assets.AsReadOnly();
    public List<ArticleActor> Actors { get; init; } = new();
}
