namespace MuhammedTask.Services.ProductReviewService.Application.ProductReviews.v1.Models;

public readonly record struct PaginatedResponse<T>
{
    private PaginatedResponse(int pageNumber, int pageSize, int totalCount, T[] items)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        Items = items;
    }

    public readonly int PageNumber { get; init; }
    public readonly int PageSize { get; init; }
    public readonly int TotalCount { get; init; }
    public readonly T[] Items { get; init; }

    public static PaginatedResponse<T> Create(int pageNumber, int pageSize, int totalCount, T[] items) =>
        new(pageNumber, pageSize, totalCount, items);
}
