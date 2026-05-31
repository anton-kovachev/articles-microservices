using Articles.Abstractions;
using Articles.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Submission.Application.Features.ApproveArticle;

namespace Submission.API.Endpoints;

public static class ApproveArticleEndpoint
{
    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/articles/{articleId:int}:approve", ([FromRoute] int articleId, [FromBody] ApproveArticleCommand command, ISender send) =>
        {
            var response = send.Send(command with { ArticleId = articleId });
            return Results.Ok(response);
        })
        .RequireAuthorization(Role.EOF, Role.REVED)
        .WithName("ApproveArticle")
        .WithTags("Articles")
        .Produces<IdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden);
    }
}
