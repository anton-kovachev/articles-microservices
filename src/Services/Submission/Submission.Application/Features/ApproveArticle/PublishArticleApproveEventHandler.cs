using Articles.IntregationEvents.Contracts;
using Articles.IntregationEvents.Contracts.Articles.Dtos;
using Mapster;
using MassTransit;
using Submission.Domain.Events;

namespace Submission.Application.Features.ApproveArticle;

public class PublishArticleApproveEventHandler(ArticleRepository _articleRepository, IPublishEndpoint _publishEndpoint) : INotificationHandler<ArticleApproved>
{
    public async Task Handle(ArticleApproved notification, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetFullArticleByIdAsync(notification.Article.Id, cancellationToken);
        var articleDto = article.Adapt<ArticleDto>();

        await _publishEndpoint.Publish(new ArticleApprovedForReviewEvent(articleDto), cancellationToken);
    }
}
