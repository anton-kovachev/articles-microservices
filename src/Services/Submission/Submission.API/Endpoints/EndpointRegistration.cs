namespace Submission.API.Endpoints;

public static class EndpointRegistration
{
    public static IEndpointRouteBuilder MappAllEndpoints(this IEndpointRouteBuilder app)
    {
        CreateArticleEndpoint.Map(app);
        AssignAuthorEndpoint.Map(app);
        CreateAndAssignAuthorEndpoint.Map(app);
        UploadManuscriptEndpoint.Map(app);
        return app;
    }
}
