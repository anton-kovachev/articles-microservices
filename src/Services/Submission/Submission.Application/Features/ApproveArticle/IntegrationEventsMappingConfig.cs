using Articles.IntregationEvents.Contracts.Articles.Dtos;
using Mapster;
using Submission.Domain.ValueObjects;

namespace Submission.Application.Features.ApproveArticle;

public class IntegrationEventsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ArticleActor, ArticleDto>()
            .Include<ArticleAuthor, ArticleDto>();

        config.NewConfig<Person, PersonDto>()
            .Include<Author, PersonDto>();

        config.ForType<string, EmailAddress>()
            .MapWith(source => EmailAddress.Create(source));
    }
}
