using Carter;
using CSharpEssentials;
using MediatR;
using MuhammedTask.BuildingBlocks.Presentation.Endpoints;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Models;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Queries.GetList;

namespace MuhammedTask.Services.ProductReviewService.WebApi.Endpoints.ProductReviews.v1.List;

public sealed class ProductReviewListEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroup = app
            .CreateVersionedGroup(Tags.ProductReviews)
            .RequireAuthorization();

        routeGroup.MapGet("product/{productId:guid}", GetProductReviews)
            .Produces<ProductReviewViewModel[]>()
            .ProducesProblem()
            .WithDescription("Get reviews by product id")
            .WithName(nameof(GetProductReviews));
    }

    private static Task<IResult> GetProductReviews(Guid productId, ISender sender, CancellationToken cancellationToken = default) =>
        sender
            .Send(new GetProductReviewListQuery(productId), cancellationToken)
            .Match(
                reviews => TypedResults.Ok(reviews),
                errors => errors.ToProblemResult(),
                cancellationToken);
}
