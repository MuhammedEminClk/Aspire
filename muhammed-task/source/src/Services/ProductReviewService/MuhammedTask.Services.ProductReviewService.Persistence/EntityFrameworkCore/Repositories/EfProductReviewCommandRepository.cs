using CSharpEssentials;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Fields;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Parameters;
using MuhammedTask.Services.ProductReviewService.Domain.ProductReviews.Repositories;
using MuhammedTask.Services.ProductReviewService.Persistence.EntityFrameworkCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MuhammedTask.Services.ProductReviewService.Persistence.EntityFrameworkCore.Repositories;

internal sealed class EfProductReviewCommandRepository(
    ApplicationWriteDbContext context) : IProductReviewCommandRepository
{
    public async Task<Result<ProductReviewId>> CreateProductReviewAsync(ProductReviewCreateParameters parameters, CancellationToken cancellationToken = default)
    {
        Result<ProductReview> reviewResult = ProductReview.Create(parameters);
        if (reviewResult.IsFailure)
            return reviewResult.Errors;
        ProductReview review = reviewResult.Value;

        await context.ProductReviews.AddAsync(review, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return review.Id;
    }

    public async Task<Result> UpdateProductReviewAsync(ProductReviewUpdateParameters parameters, CancellationToken cancellationToken = default)
    {
        ProductReview? found = await context.ProductReviews
            .Where(r => r.Id == parameters.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (found is null)
            return ProductReviewErrors.NotFoundError(parameters.Id);

        Result updateResult = found.Update(parameters);
        if (updateResult.IsFailure)
            return updateResult.Errors;

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteProductReviewAsync(ProductReviewId productReviewId, UserId userId, CancellationToken cancellationToken = default)
    {
        ProductReview? found = await context.ProductReviews
            .Where(r => r.Id == productReviewId)
            .FirstOrDefaultAsync(cancellationToken);

        if (found is null)
            return ProductReviewErrors.NotFoundError(productReviewId);

        Result deleteResult = found.Delete(userId);
        if (deleteResult.IsFailure)
            return deleteResult.Errors;

        context.ProductReviews.Remove(found);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
