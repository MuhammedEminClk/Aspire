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
            .CreateVersionedGroup(Tags.Products)
            .RequireAuthorization();

        routeGroup.MapGet("{productId:guid}/reviews", GetProductReviews)
            .Produces<PaginatedResponse<ProductReviewViewModel>>()
            .ProducesProblem()
            .WithDescription("Get paginated reviews by product id")
            .WithName(nameof(GetProductReviews));
    }

    private static Task<IResult> GetProductReviews(
        Guid productId,
        ISender sender,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default) =>
        sender
            .Send(new GetProductReviewListQuery(productId, pageNumber, pageSize), cancellationToken)
            .Match(
                reviews => TypedResults.Ok(reviews),
                errors => errors.ToProblemResult(),
                cancellationToken);
}
