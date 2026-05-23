using FluentValidation;

namespace MuhammedTask.Services.CategoryService.Application.Categories.v1.Queries.Exist;

internal sealed class CategoryExistQueryValidator : AbstractValidator<CategoryExistQuery>
{
    public CategoryExistQueryValidator() => RuleFor(x => x.CategoryId).NotEmpty();
}
