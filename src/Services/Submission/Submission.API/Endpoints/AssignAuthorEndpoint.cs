using MediatR;
using Articles.Abstractions.Enums;
using Articles.Security;
using Submission.Application.Features.AssignAuthor;

namespace Submission.API.Endpoints
{
    public static class AssignAuthorEndpoint
    {
        public static void Map(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/articles/{articleId:int}/authors/{authorId:int}", async (int articleId, int authorId, AssignAuthorCommand command, ISender sender) =>
            {
                var response = await sender.Send(command with { ArticleId = articleId, AuthorId = authorId });
                return Results.Ok(response);
            })
            .RequireRoleAuthorization(Role.CORAUT)
            .WithName("AssignAuthor")
            .WithTags("Articles")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);
        }
    }
}
