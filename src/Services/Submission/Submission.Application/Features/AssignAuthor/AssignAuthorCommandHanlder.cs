using Blocks.EntityFrameworkCore;

namespace Submission.Application.Features.AssignAuthor;

public class AssignAuthorCommandHanlder(ArticleRepository _articleRepository) : IRequestHandler<AssignAuthorCommand, IdResponse>
{
    public async Task<IdResponse> Handle(AssignAuthorCommand command, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdOrThrowAsync(command.ArticleId);
        var author = await _articleRepository.DbContext.Authors.FindByIdOrThrowAsync(command.AuthorId);

        article.AssignAuthor(author, command.ContributionAreas, command.IsCorrespondingAuthor);

        await _articleRepository.SaveChangesAsync();

        return new IdResponse(article.Id);
    }
}
