using Carter;
using CSharpEssentials;
using MediatR;
using MuhammedTask.BuildingBlocks.Presentation.Endpoints;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Models;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Queries.GetAverageRating;

namespace MuhammedTask.Services.ProductReviewService.WebApi.Endpoints.ProductReviews.v1.AverageRating;

public sealed class ProductReviewAverageRatingEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroup = app
            .CreateVersionedGroup(Tags.Products)
            .RequireAuthorization();

        routeGroup.MapGet("{productId:guid}/average-rating", GetAverageRating)
            .Produces<ProductAverageRatingViewModel>()
            .ProducesProblem()
            .WithDescription("Get average rating by product id")
            .WithName(nameof(GetAverageRating));
    }

    private static Task<IResult> GetAverageRating(Guid productId, ISender sender, CancellationToken cancellationToken = default) =>
        sender
            .Send(new GetProductAverageRatingQuery(productId), cancellationToken)
            .Match(
                result => TypedResults.Ok(result),
                errors => errors.ToProblemResult(),
                cancellationToken);
}
