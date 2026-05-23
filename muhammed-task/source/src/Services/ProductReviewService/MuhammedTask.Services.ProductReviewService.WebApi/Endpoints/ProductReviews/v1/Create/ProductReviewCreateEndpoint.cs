using Carter;
using CSharpEssentials;
using MediatR;
using MuhammedTask.BuildingBlocks.Presentation.Endpoints;
using MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Commands.Create;
using Microsoft.AspNetCore.Mvc;

namespace MuhammedTask.Services.ProductReviewService.WebApi.Endpoints.ProductReviews.v1.Create;

public sealed class ProductReviewCreateEndpoint : CarterModule
{
    public sealed record ProductReviewCreateRequest(Guid ProductId, Guid UserId, int? Rating, string? Comment)
    {
        public CreateProductReviewCommand ToCommand() =>
            new(ProductId, UserId, Rating, Comment);
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroup = app
            .CreateVersionedGroup(Tags.ProductReviews)
            .RequireAuthorization();

        routeGroup.MapPost(string.Empty, CreateProductReview)
            .Produces<Guid>(HttpCodes.Created)
            .ProducesProblem()
            .WithDescription("Create product review")
            .WithName(nameof(CreateProductReview));
    }

    private static Task<IResult> CreateProductReview([FromBody] ProductReviewCreateRequest request, ISender sender, CancellationToken cancellationToken = default) =>
        sender
            .Send(request.ToCommand(), cancellationToken)
            .Match(
                review => TypedResults.Created($"/{Tags.ProductReviews}/{review.Value}", review.Value),
                errors => errors.ToProblemResult(),
                cancellationToken);
}
