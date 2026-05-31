using Blocks.Domain;
using Submission.Domain.Events;

namespace Submission.Domain.Entities;

public partial class Article
{
    public void AssignAuthor(Author author, HashSet<ContributionArea> contributionAreas, bool isCorrespondingAuthor)
    {
        var role = isCorrespondingAuthor ? UserRoleType.CORAUT : UserRoleType.AUT;

        if (Actors.Exists(a => a.PersonId == author.Id && a.Role == role))
            throw new DomainException($"Author {author.Email} is already assigned to the article");

        Actors.Add(new ArticleAuthor 
        {
            Person = author,
            ContributionAreas = contributionAreas,
            Role = role
        });

        //TODO: Create Domain Event
    }

    public Asset CreateAsset(AssetTypeDefinition type)
    {
        var assetCount = this.Assets.Where(a => a.Type == type.Id).Count();     

        if (assetCount >= type.MaxAssetCount)
            throw new DomainException($"Cannot add more than {type.MaxAssetCount} assets of type {type.Name} to the article");

        var asset = Asset.Create(this, type);
        _assets.Add(asset);
        return asset;
    }

    public void Submit(IArticleAction<ArticleActionType> action, ArticleStateMachineFactory _stateMachineFactory)
    {
        this.SubmittedById = action.CreatedById;
        this.SubmittedOn = action.CreatedOn;

        //TODO: Add Submit Domain Event
        //SetStage(ArticleStage.Submitted, action, _stateMachineFactory);
    }

    public void Approve(Person person)
    {
        this.Actors.Add(new ArticleActor 
        { 
            Person = person ,
            Role = UserRoleType.REVED
        });

        AddDomainEvent(new ArticleApproved(this));
    }
}

public class ArticleStateMachineFactory { }
