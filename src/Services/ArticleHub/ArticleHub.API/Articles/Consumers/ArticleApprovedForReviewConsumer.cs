using ArticleHub.Domain.Articles;
using ArticleHub.Persistence;
using Articles.IntregationEvents.Contracts;
using Articles.IntregationEvents.Contracts.Articles.Dtos;
using Blocks.Core.Mapster;
using Blocks.Exceptions;
using Mapster;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ArticleHub.API.Articles.Consumers
{
    public sealed class ArticleApprovedForReviewConsumer(ArticleHubDbContext _dbContext) : IConsumer<ArticleApprovedForReviewEvent>
    {
        public async Task Consume(ConsumeContext<ArticleApprovedForReviewEvent> context)
        {
            var articleDto = context.Message.Article;

            if (await _dbContext.Articles.AnyAsync(e => e.Id == articleDto.Id, context.CancellationToken))
                throw new BadRequestException("Article was already approved for review.");

            var journal = await GetOrCreateJournalAsync(articleDto, context.CancellationToken);

            var article = articleDto.AdaptWith<Article>(a =>
            {
                a.Journal = journal;
                a.SubmittedById = articleDto.SubmittedBy.Id;
            });

            await CreateActorsAsync(articleDto, article, context.CancellationToken);

            await _dbContext.Articles.AddAsync(article, context.CancellationToken);
            await _dbContext.SaveChangesAsync(context.CancellationToken);
        }

        private async Task<Journal> GetOrCreateJournalAsync(ArticleDto articleDto, CancellationToken cancellationToken = default)
        {
            var journal = await _dbContext.Journals.SingleOrDefaultAsync(e => e.Id == articleDto.Journal.Id, cancellationToken);
            if (journal is null)
            {
                journal = articleDto.Journal.Adapt<Journal>();
                await _dbContext.AddAsync(journal, cancellationToken);
            }

            return journal;
        }

        private async Task CreateActorsAsync(ArticleDto articleDto, Article article, CancellationToken cancellationToken = default)
        {
            foreach (var actorDto in articleDto.Actors)
            {
                var person = await _dbContext.Persons.SingleOrDefaultAsync(e => e.UserId == actorDto.Person.Id, cancellationToken);
                if (person is null)
                {
                    person = actorDto.Person.Adapt<Person>();
                    await _dbContext.AddAsync(person, cancellationToken);
                }

                article.Actors.Add(new ArticleActor
                {
                    ArticleId = article.Id,
                    Article = article,
                    PersonId = person.Id,
                    Person = person,
                    Role = actorDto.Role
                });
            }
        }
    }
}
