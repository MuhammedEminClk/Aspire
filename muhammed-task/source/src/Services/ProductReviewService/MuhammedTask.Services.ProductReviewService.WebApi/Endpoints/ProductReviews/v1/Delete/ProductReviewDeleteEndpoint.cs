using Carter;
using CSharpEssentials;
using MediatR;
using MuhammedTask.BuildingBlocks.Presentation.Endpoints;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Commands.Delete;

namespace MuhammedTask.Services.ProductReviewService.WebApi.Endpoints.ProductReviews.v1.Delete;

public sealed class ProductReviewDeleteEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroup = app
            .CreateVersionedGroup(Tags.ProductReviews)
            .RequireAuthorization();

        routeGroup.MapDelete("{reviewId:guid}", DeleteProductReview)
            .Produces(HttpCodes.NoContent)
            .ProducesProblem()
            .WithDescription("Delete product review")
            .WithName(nameof(DeleteProductReview));
    }

    private static Task<IResult> DeleteProductReview(Guid reviewId, Guid userId, ISender sender, CancellationToken cancellationToken = default) =>
        sender
            .Send(new DeleteProductReviewCommand(reviewId, userId), cancellationToken)
            .Match(
                TypedResults.NoContent,
                errors => errors.ToProblemResult(),
                cancellationToken);
}
