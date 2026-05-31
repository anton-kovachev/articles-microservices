using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace ArticleHub.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ArticleHubDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));


        var hasuraOptions = configuration.GetSectionByTypeName<HasuraOptions>();

        services.AddSingleton(_ =>
        {
            var graphQlHttpClientOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(hasuraOptions.Enpoint)
            };

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            var graphQlHttpClient = new GraphQLHttpClient(graphQlHttpClientOptions, new SystemTextJsonSerializer(jsonSerializerOptions));
            graphQlHttpClient.HttpClient.DefaultRequestHeaders.Add("x-hasura-admin-secret", hasuraOptions.AdminSecret);

            return graphQlHttpClient;
        });

        services.AddScoped<ArticleGraphQLReadStore>();

        return services;
    }
}
