using MediatR;
using Articles.Security;
using Articles.Abstractions.Enums;
using Submission.Application.Features.CreateAndAssignAuthor;

namespace Submission.API.Endpoints;

public static class CreateAndAssignAuthorEndpoint
{
    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/article/{articleId:int}/authors", async (int articleId, CreateAndAssignAuthorCommand command, ISender sender) =>
        {
            var response = await sender.Send(command with { ArticleId = articleId });
            return Results.Ok(response);
        })
        .RequireRoleAuthorization(Role.CORAUT)
        .WithName("CreateAndAssignAuthor")
        .WithTags("Articles")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest);
    }
}
