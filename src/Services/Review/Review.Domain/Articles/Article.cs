using Articles.Abstractions.Enums;
using Articles.IntregationEvents.Contracts.Articles.Dtos;
using Blocks.Domain.Entities;
using Review.Domain.Assets;
using Review.Domain.Invitations;

namespace Review.Domain.Articles;

public partial class Article : AggregateRoot
{
    public required string Title { get; set; }
    public ArticleType Type { get; init; }
    public string Scope { get; init; } = default!;
    public DateTime? SubmittedOn { get; init; }
    public int? SubmittedById { get; init; }    
    public PersonDto? SubmittedBy { get; set; }
    public ArticleStage Stage { get; private set; }
    public required int JournalId { get; init; }
    public Journal Journal { get; init; } = null!;
    private readonly List<ArticleActor> _actors = new();
    public Editor Editor => (Editor)_actors.Single(a => a.Role == UserRoleType.REVED).Person;
    public IReadOnlyList<ArticleActor> Actors => _actors.AsReadOnly();
    private readonly List<Asset> _assets = new();
    public IReadOnlyList<Asset> Assets => _assets.AsReadOnly();
    private readonly List<ReviewInvitation> _invitations = new();
    public IReadOnlyList<ReviewInvitation> Invitations => _invitations.AsReadOnly();
}
