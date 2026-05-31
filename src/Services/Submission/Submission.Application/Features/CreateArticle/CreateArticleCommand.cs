using Blocks.Core.FluentValidation;

namespace Submission.Application.Features.CreateArticle;

public record CreateArticleCommand(int JournalId, string Title, string Scope, ArticleType ArticleType) : ArticleCommand
{
    public override ArticleActionType ActionType => ArticleActionType.Create;
};

public class CreateArticleCommandValidator : ArticleCommandValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(x => x.Title).NotEmptyWithMessage("Title cannot be empty");
        RuleFor(x => x.Scope).NotEmptyWithMessage("Scope cannot be empty");
        RuleFor(x => x.JournalId).GreaterThan(0).WithMessageForInvalidId("JournalId");
    }
}
