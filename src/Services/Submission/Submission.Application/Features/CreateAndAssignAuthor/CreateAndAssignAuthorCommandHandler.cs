
using Blocks.EntityFrameworkCore;
using MediatR;
namespace Submission.Application.Features.CreateAndAssignAuthor;

public class CreateAndAssignAuthorCommandHandler(ArticleRepository _articleRepository)
    : IRequestHandler<CreateAndAssignAuthorCommand, IdResponse>
{
    public async Task<IdResponse> Handle(CreateAndAssignAuthorCommand command, CancellationToken cancellationToken)
    {
        Article article = await _articleRepository.GetByIdOrThrowAsync(command.ArticleId);
        Author? author = null;

        if (command.UserId is null)
        {
            author = Author.Create(command.FirstName!, command.LastName!, command.Email!, command.Title, command.Affiliation!);
        } else
        {

        }

        article.AssignAuthor(author, command.ContributionAreas, command.IsCorrespondingAuthor);
        await _articleRepository.SaveChangesAsync();

        return new IdResponse(article.Id);
    }
}
