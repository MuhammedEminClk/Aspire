using FluentValidation;
using MuhammedTask.Services.CategoryService.Domain.Categories.Fields;

namespace MuhammedTask.Services.CategoryService.Application.Categories.v1.Commands.Create;

internal sealed class CategoryCreateCommandValidator : AbstractValidator<CategoryCreateCommand>
{
    public CategoryCreateCommandValidator() =>
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(CategoryName.MinLength)
            .MaximumLength(CategoryName.MaxLength);
}
