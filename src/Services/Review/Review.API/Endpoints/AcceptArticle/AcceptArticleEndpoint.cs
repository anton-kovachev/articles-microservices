using Articles.Security;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Review.Application.Features.Articles.AcceptArticle;

namespace Review.API.Endpoints.AcceptArticle
{
    public class AcceptArticleEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/articles/{articleId:int}:accept", async ([FromRoute] int articleId, [FromBody] AcceptArticleCommand command, ISender sender) =>
            {
                await sender.Send(command with { ArticleId = articleId });
            })
            .RequireAuthorization(Role.REVED)
            .Produces<AcceptArticleResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
}
