using MediatR;

namespace Review.Application.Features.Articles.AcceptArticle;

public class AcceptArticleCommandHandler(
    ReviewDbContext _reviewDbContext,
    ArticleRepository _articleRepository) : IRequestHandler<AcceptArticleCommand, AcceptArticleResponse>
{
    public async Task<AcceptArticleResponse> Handle(AcceptArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdOrThrowAsync(command.ArticleId, cancellationToken);
        article.AcceptForProduction(command);

        await _reviewDbContext.SaveChangesAsync(cancellationToken);
        return new AcceptArticleResponse(article.Id, article.Editor.Id, DateTime.UtcNow);
    }
}
