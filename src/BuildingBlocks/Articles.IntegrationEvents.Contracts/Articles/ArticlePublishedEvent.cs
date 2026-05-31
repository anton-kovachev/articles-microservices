using Articles.IntregationEvents.Contracts.Articles.Dtos;

namespace Articles.IntregationEvents.Contracts;

public record ArticlePublishedEvent(ArticleDto Article);
