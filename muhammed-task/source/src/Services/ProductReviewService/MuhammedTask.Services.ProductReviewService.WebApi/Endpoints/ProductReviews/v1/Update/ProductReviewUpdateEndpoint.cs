using Carter;
using CSharpEssentials;
using MediatR;
using MuhammedTask.BuildingBlocks.Presentation.Endpoints;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Commands.Update;
using Microsoft.AspNetCore.Mvc;

namespace MuhammedTask.Services.ProductReviewService.WebApi.Endpoints.ProductReviews.v1.Update;

public sealed class ProductReviewUpdateEndpoint : CarterModule
{
    public sealed record ProductReviewUpdateRequest(Guid UserId, int? Rating, string? Comment)
    {
        public UpdateProductReviewCommand ToCommand(Guid id) =>
            new(id, UserId, Rating, Comment);
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroup = app
            .CreateVersionedGroup(Tags.ProductReviews)
            .RequireAuthorization();

        routeGroup.MapPut("{reviewId:guid}", UpdateProductReview)
            .Produces(HttpCodes.NoContent)
            .ProducesProblem()
            .WithDescription("Update product review")
            .WithName(nameof(UpdateProductReview));
    }

    private static Task<IResult> UpdateProductReview(Guid reviewId, [FromBody] ProductReviewUpdateRequest request, ISender sender, CancellationToken cancellationToken = default) =>
        sender
            .Send(request.ToCommand(reviewId), cancellationToken)
            .Match(
                TypedResults.NoContent,
                errors => errors.ToProblemResult(),
                cancellationToken);
}
