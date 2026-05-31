using Articles.IntregationEvents.Contracts.Articles.Dtos;
using Blocks.Core.GrpahQl;
using GraphQL;
using GraphQL.Client.Http;

namespace ArticleHub.Persistence;

public class ArticleGraphQLReadStore(GraphQLHttpClient _graphQLHttpClient)
{
    public async Task<QueryResult<ArticleDto>> GetArticlesAsync(object filter, int limit = 20, int offset = 0, CancellationToken cancellationToken = default)
    {
        var request = new GraphQLRequest
        {
            Query = @"
            query GetArticles($filter: artilce_bool_exp) {
                items: article(where: $filter) {
                    id
                    title
                    doi
                    type
                    stage
                    submittedon
                    publishedon
                    acceptedon,
                    journal {
                        name,
                        abbreviation
                    },
                    submittedby:person {
                        id,
                        name
                    }
            }
            ",
            Variables = new { filter, limit, offset }
        };

        var response = await _graphQLHttpClient.SendQueryAsync<QueryResult<ArticleDto>>(request, cancellationToken);
        return response.Data;
    }
}
