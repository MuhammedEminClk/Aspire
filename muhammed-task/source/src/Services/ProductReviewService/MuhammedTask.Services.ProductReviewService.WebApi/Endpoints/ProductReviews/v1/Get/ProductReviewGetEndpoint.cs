using Carter;
using CSharpEssentials;
using MediatR;
using MuhammedTask.BuildingBlocks.Presentation.Endpoints;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Models;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Queries.GetById;

namespace MuhammedTask.Services.ProductReviewService.WebApi.Endpoints.ProductReviews.v1.Get;

public sealed class ProductReviewGetEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroup = app
            .CreateVersionedGroup(Tags.ProductReviews)
            .RequireAuthorization();

        routeGroup.MapGet("{reviewId:guid}", GetProductReview)
            .Produces<ProductReviewViewModel>()
            .ProducesProblem()
            .WithDescription("Get product review by id")
            .WithName(nameof(GetProductReview));
    }

    private static Task<IResult> GetProductReview(Guid reviewId, ISender sender, CancellationToken cancellationToken = default) =>
        sender
            .Send(new GetProductReviewByIdQuery(reviewId), cancellationToken)
            .Match(
                review => TypedResults.Ok(review),
                errors => errors.ToProblemResult(),
                cancellationToken);
}
