using Mapster;
using MassTransit;
using MediatR;
using Articles.IntregationEvents.Contracts;
using Articles.IntregationEvents.Contracts.Articles.Dtos;
using Review.Domain.Articles.Events;

namespace Review.Application.Features.Articles.AcceptArticle;

public class PublishArticleAcceptForProductioHandler(IPublishEndpoint _publishEndpoint) : INotificationHandler<AcceptForProduction>
{
    public async Task Handle(AcceptForProduction notification, CancellationToken cancellationToken)
    {
        var articleAcceptForProductionIntegrationEvent = new ArticleAcceptedForProductionEvent(notification.Article.Adapt<ArticleDto>());
        await _publishEndpoint.Publish(articleAcceptForProductionIntegrationEvent, cancellationToken);
    }
}
