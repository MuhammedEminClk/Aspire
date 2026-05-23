using Carter;
using CSharpEssentials;
using MediatR;
using MuhammedTask.BuildingBlocks.Presentation.Endpoints;
using MuhammedTask.Services.CategoryService.Application.Categories.v1.Queries.Get;
using MuhammedTask.Services.CategoryService.Application.Products.v1.Models;

namespace MuhammedTask.Services.CategoryService.WebApi.Endpoints.Categories.v1.Get;

public sealed class CategoryGetEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroup = app
            .CreateVersionedGroup(Tags.Categories)
            .RequireAuthorization();

        routeGroup.MapGet("{categoryId:guid}", GetCategory)
            .Produces<CategoryViewModel>()
            .ProducesProblem()
            .WithDescription("Get Category by id")
            .WithName(nameof(GetCategory));
    }

    private static Task<IResult> GetCategory(Guid categoryId, ISender sender, CancellationToken cancellationToken = default) =>
            sender
               .Send(new GetCategoryQuery(categoryId), cancellationToken)
               .Match(
                     Category => TypedResults.Ok(Category),
                     errors => errors.ToProblemResult(),
                     cancellationToken);
}
