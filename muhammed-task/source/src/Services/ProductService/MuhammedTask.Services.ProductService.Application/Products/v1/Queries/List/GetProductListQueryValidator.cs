using FluentValidation;

namespace MuhammedTask.Services.ProductService.Application.Products.v1.Queries.List;

internal sealed class GetProductListQueryValidator : AbstractValidator<GetProductListQuery>
{
    public GetProductListQueryValidator() => RuleFor(x => x.CategoryId).NotEmpty();
}
