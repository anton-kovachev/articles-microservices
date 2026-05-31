using ArticleHub.Domain.Dtos;
using ArticleHub.Persistence;
using Blocks.Core.GrpahQl;
using Carter;

namespace ArticleHub.API.Articles.SearchArticles
{
    public class SearchArticlesEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/articles/graphql", async (SearchArticlesQuery articlesQuery, ArticleGraphQLReadStore _graphQLReadStore, CancellationToken cancellationToken) =>
            {
                var result = await _graphQLReadStore.GetArticlesAsync(
                    articlesQuery.Filter, 
                    articlesQuery.Pagination.Limit, 
                    articlesQuery.Pagination.Offset, 
                    cancellationToken);

                return Results.Json(result);
            })
            .RequireAuthorization()
            .WithName("GetArticles")
            .WithTags("Articles")
            .Produces<IEnumerable<ArticleDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);
        }
    }
}
