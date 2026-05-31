using Articles.Abstractions;
using Review.Application.Features.Shared;
using Review.Domain.Shared.Enums;

namespace Review.Application.Features.Articles.AcceptArticle;

public record AcceptArticleCommand() : ArticleCommand<ArticleActionType, AcceptArticleResponse>
{
    public override ArticleActionType ActionType => ArticleActionType.AcceptArticleForProduction;
}

public record AcceptArticleResponse(int ArticleId, int AcceptedById, DateTime AcceptedOn);

public class AcceptArticleCommandValidator : ArticleCommandValidator<AcceptArticleCommand>
{ 
    public AcceptArticleCommandValidator()
    {
    }
}

