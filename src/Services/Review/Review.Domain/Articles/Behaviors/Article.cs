using Articles.Abstractions;
using Articles.Abstractions.Enums;
using Articles.IntregationEvents.Contracts.Articles.Dtos;
using Auth.Grpc;
using Blocks.Domain;
using Review.Domain.Articles.Events;
using Review.Domain.Assets;
using Review.Domain.Invitations;
using Review.Domain.Invitations.ValueObjects;
using Review.Domain.Reviewers;

namespace Review.Domain.Articles;

public partial class Article
{
    public static Article AcceptSubmitted(ArticleDto articleDto, IEnumerable<ArticleActor> actors, IEnumerable<Asset> assets)
    {
        var article = new Article
        {
            Id = articleDto.Id,
            JournalId = articleDto.Journal.Id,
            Title = articleDto.Title,
            Type = articleDto.Type,
            Scope = articleDto.Scope,
            SubmittedById = articleDto.SubmittedBy.Id,
            SubmittedOn = articleDto.SubmittedOn,
            Stage = articleDto.Stage
        };
        
        article._actors.AddRange(actors);
        article._assets.AddRange(assets);
        return article;
    }

    public ReviewInvitation InviteReviewer(Reviewer reviewer, IArticleAction action)
    {
        if (!reviewer.Specializations.Any(s => s.JournalId == JournalId))
            throw new DomainException($"Reviewer {reviewer.FullName} is not specialized in this journal");

        return CreateInvitation(reviewer.Id, reviewer.Email, reviewer.FirstName, reviewer.LastName, action);
    }

    public ReviewInvitation InviteReviewer(int? userId, string email, string firstName, string lastName, IArticleAction action)
    {
        return CreateInvitation(userId, email, firstName, lastName, action);
    }

    private ReviewInvitation CreateInvitation(int? userId, string email, string firstName, string lastName, IArticleAction action)
    {
        if (_invitations.Any(i => i.Email.Value.Trim().ToUpperInvariant() == email && !i.IsExpired))
            throw new DomainException($"Reviewer {firstName} {lastName} with email {email} was already invited");

        var invitation = new ReviewInvitation
        {
            ArticleId = Id,
            UserId = userId,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            SentById = action.CreatedById,
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            Token = InvitationToken.CreateNew()
        };

        _invitations.Add(invitation);
        AddDomainEvent(new ReviewerInvited(this, invitation));
        return invitation;
    }

    public void AssignReviewer(Reviewer reviewer, IArticleAction action)
    {
        if (Actors.Any(a => a.PersonId == reviewer.Id && a.Role == UserRoleType.REV))
            throw new DomainException($"Reviewer with email {reviewer.Email} is already assigned as a reviewer");

        reviewer.AddSpecialization(new ReviewerSpecialization { JournalId = JournalId, ReviewerId = reviewer.Id });

        _actors.Add(new ArticleActor
        {
            ArticleId = this.Id,
            Article = this,
            PersonId = reviewer.Id,
            Person = reviewer,
            Role = UserRoleType.REV,
        });

        AddDomainEvent(new ReviewerAssigned(this, reviewer, action));
    }

    public void AcceptForProduction(IArticleAction action)
    {
        Stage = ArticleStage.Approved;
        AddDomainEvent(new AcceptForProduction(this, Editor, action));
    }
}
