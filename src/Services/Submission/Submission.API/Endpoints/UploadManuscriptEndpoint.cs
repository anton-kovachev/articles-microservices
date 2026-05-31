using MediatR;
using Articles.Abstractions.Enums;
using Articles.Security;
using Submission.Application.Features.UploadFile.UploadManuscriptFile;
using Microsoft.AspNetCore.Mvc;

namespace Submission.API.Endpoints
{
    public static class UploadManuscriptEndpoint
    {
        public static void Map(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/article{articleId:int}/assets/manuscript:upload", async ([FromRoute] int articleId, [FromForm] UploadManuscriptFileCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);
                return Results.Ok($"/api/article/{command.ArticleId}/assets/{response.Id}/download/{response.Id}");
            })
            .RequireRoleAuthorization(UserRoleType.CORAUT)
            .WithName("UploadManuscript")
            .WithTags("assets")
            .Accepts<IFormFile>("multipart/form-data")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery(); //because of IFormFile
        }
    }
}
