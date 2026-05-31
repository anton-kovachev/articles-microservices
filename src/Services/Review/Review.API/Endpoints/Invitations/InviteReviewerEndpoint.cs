using Articles.Abstractions;
using Articles.Security;
using Carter;
using Grpc.Core;
using MediatR;
using Review.Application.Features.Invitations.InviteReviewer;

namespace Review.API.Endpoints.Invitations;

public class InviteReviewerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/articles/{articleId:int}/invitations", async (int articleId, InviteReviewerResponse command, ISender sender) =>
        {
            command = command with { ArticleId = articleId };
            var response = await sender.Send(command);

            return Results.Ok(response);
        })
        .RequireRoleAuthorization(Role.REVED, Role.EOF)
        .WithName("Invite Reviewer")
        .WithTags("Invitations")
        .Produces<IdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status401Unauthorized);    
    }
}
