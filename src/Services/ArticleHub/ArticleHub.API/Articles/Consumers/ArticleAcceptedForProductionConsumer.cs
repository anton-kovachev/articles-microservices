using ArticleHub.Persistence;
using Articles.Abstractions.Enums;
using Articles.IntregationEvents.Contracts;
using Blocks.Core;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ArticleHub.API.Articles.Consumers
{
    public sealed class ArticleAcceptedForProductionConsumer(ArticleHubDbContext _dbContext) : IConsumer<ArticleAcceptedForProductionEvent>
    {
        public async Task Consume(ConsumeContext<ArticleAcceptedForProductionEvent> context)
        {
            var article = Guard.NotFound(await _dbContext.Articles.SingleOrDefaultAsync(a => a.Id == context.Message.Article.Id, context.CancellationToken));
            if (article.Stage != ArticleStage.Submitted)
                throw new Exception($"Article {article.Title} is in stage {article.Stage}");

            article.Stage = ArticleStage.Approved;
            await _dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}
