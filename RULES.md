# Kodlama Kuralları — ProductReview Microservice

Bu dosya Task.md'deki gereksinimleri ve mevcut ProductService/CategoryService pattern'lerini
referans alarak hazırlanmıştır. Kod yazarken bu kurallara eksiksiz uyulacaktır.

---

## 1. Genel Prensipler

### 1.1 Pattern Taklit Zorunluluğu
- Mevcut `MuhammedTask.Services.ProductService.*` pattern'lerini **birebir** taklit et.
- Yeni mimari, yeni kütüphane, yeni kalıp **icat etme**.
- Namespace: `MuhammedTask.Services.ProductReviewService.*`

### 1.2 Exception Yasağı
```csharp
// YASAK
throw new Exception("...");
throw new InvalidOperationException("...");

// DOĞRU
return ProductReviewErrors.NotFoundError(id);
return Result.Failure(ProductReviewErrors.AlreadyReviewedError);
```

### 1.3 Async Kuralları
```csharp
// YASAK — deadlock riski
var result = someTask.Result;
someTask.Wait();

// DOĞRU
var result = await someTask;
```

---

## 2. Domain Layer Kuralları

### 2.1 Value Objects
- **Tüm** ID'ler ve anlamlı alanlar value object olacak: `ProductReviewId`, `ProductId`, `UserId`, `ReviewRating`, `ReviewComment`
- `string userId` gibi primitive obsession **yasak**
- Her value object: `readonly record struct`, private ctor, `From()` + `Create()` factory method, `Empty` static field

```csharp
// DOĞRU
public readonly record struct ReviewRating
{
    public const int MinValue = 1;
    public const int MaxValue = 5;
    private ReviewRating(int value) => Value = value;
    public int Value { get; }
    public static ReviewRating From(int value) => new(value);
    public static Result<ReviewRating> Create(int? value)
    {
        if (value is null) return ProductReviewErrors.Rating.EmptyError;
        if (value < MinValue || value > MaxValue) return ProductReviewErrors.Rating.OutOfRangeError(value.Value);
        return new ReviewRating(value.Value);
    }
}
```

### 2.2 Aggregate Root — Rich Domain (Anemic Domain Yasak)
- Business logic **entity'de** olacak, handler'da değil
- `ProductReview.Create()` → validation + domain event raise içerir
- `ProductReview.Update()` → validation + domain event raise içerir
- `ProductReview.Delete()` → domain event raise eder

```csharp
// YASAK — Anemic Domain
public class ProductReview { public int Rating { get; set; } }
// Handler'da: productReview.Rating = newRating; // Bu anti-pattern

// DOĞRU — Rich Domain
public sealed class ProductReview : SoftDeletableEntityBase<ProductReviewId>
{
    public static Result<ProductReview> Create(ProductReviewCreateParameters parameters) { /* validation */ }
    public Result Update(ProductReviewUpdateParameters parameters) { /* validation + Raise(event) */ }
    public void Delete() => Raise(new ProductReviewDeletedDomainEvent(Id));
}
```

### 2.3 Domain Events
- Domain event'ler sadece aggregate içinde `Raise()` ile publish edilir
- Handler dışında `IPublisher` ile publish etmek **yasak**
- Interceptor (`PublishDomainEventsInterceptor`) SaveChanges sırasında otomatik publish eder

### 2.4 Errors
```csharp
// ProductReviewErrors static sınıfı — nested class'lar ile organize edilir
public static class ProductReviewErrors
{
    public static Error NotFoundError(ProductReviewId id) => Error.NotFound(...);
    public static readonly Error AlreadyReviewedError = Error.Conflict(...);

    public static class Rating
    {
        public static readonly Error EmptyError = Error.Validation(...);
        public static Error OutOfRangeError(int value) => Error.Validation(...);
    }
    public static class Comment
    {
        public static Error TooLongError(int length) => Error.Validation(...);
    }
}
```

### 2.5 İş Kuralları
- Aynı kullanıcı aynı `ProductId` için 2. review **yapamaz** → `AlreadyReviewedError`
- Kullanıcı yalnızca **kendi** review'ını update/delete edebilir → `UserId` match check

---

## 3. Application Layer Kuralları

### 3.1 Command Pattern
```csharp
// Command: ICommand<TResponse> implement eder
public sealed record CreateProductReviewCommand(
    Guid ProductId,
    Guid UserId,
    int? Rating,
    string? Comment) : ICommand<ProductReviewId>
{
    public ProductReviewCreateParameters ToParameters() =>
        new(ProductId.From(ProductId), UserId.From(UserId), Rating, Comment);
}

// Handler: internal sealed class, primary constructor
internal sealed class CreateProductReviewCommandHandler(
    IProductReviewCommandRepository repository) : ICommandHandler<CreateProductReviewCommand, ProductReviewId>
{
    public Task<Result<ProductReviewId>> Handle(...) { ... }
}
```

### 3.2 Query Pattern
```csharp
// Query: ICachedQuery<TViewModel> implement eder — CacheKey, Tags, Expiration zorunlu
public sealed record GetProductReviewByIdQuery(Guid ProductReviewId) : ICachedQuery<ProductReviewViewModel>
{
    public bool BypassCache => false;
    public bool CacheFailures => true;
    public string CacheKey => $"productreview:{ProductReviewId}";
    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    public string[] Tags => [];
}

// Handler: ICachedQueryHandler implement eder
internal sealed class GetProductReviewByIdQueryHandler(
    IProductReviewQueryRepository repository) : ICachedQueryHandler<GetProductReviewByIdQuery, ProductReviewViewModel>
{
    public async Task<Result<ProductReviewViewModel>> Handle(...) { ... }
}
```

### 3.3 Validator
```csharp
// internal sealed class, AbstractValidator<T>
internal sealed class CreateProductReviewCommandValidator : AbstractValidator<CreateProductReviewCommand>
{
    public CreateProductReviewCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Rating).NotNull().InclusiveBetween(ReviewRating.MinValue, ReviewRating.MaxValue);
        RuleFor(x => x.Comment).MaximumLength(ReviewComment.MaxLength);
    }
}
```

### 3.4 ViewModel
```csharp
// readonly record struct, private ctor, Create factory
public readonly record struct ProductReviewViewModel
{
    private ProductReviewViewModel(ProductReviewReadModel r) { /* field assignment */ }
    public static ProductReviewViewModel Create(ProductReviewReadModel r) => new(r);
    public static ProductReviewViewModel[] Create(ProductReviewReadModel[] rs) => [.. rs.Select(Create)];
}
```

### 3.5 Domain Event Handler
```csharp
// INotificationHandler<TEvent>, cache invalidate
internal sealed class ProductReviewCreatedDomainEventHandler(
    ILogger<...> logger,
    ICacheService cacheService) : INotificationHandler<ProductReviewCreatedDomainEvent>
{
    private const string Tag = "productreviews";
    public async Task Handle(...) => await cacheService.InvalidateTagAsync(Tag);
}
```

---

## 4. Persistence Layer Kuralları

### 4.1 DbContext
- Write: `WriteDbContextBase<ApplicationWriteDbContext>`, `ApplySoftDeleteQueryFilter()`
- Read: `ReadDbContextBase<ApplicationReadDbContext>`, `ApplySoftDeleteQueryFilter()`, `QueryTrackingBehavior.NoTracking`

### 4.2 EF Configuration
```csharp
// Write configuration
builder.SoftDeletableEntityBaseMap<ProductReview, ProductReviewId>();
builder.Property(p => p.Id).HasConversion(id => id.Value, id => ProductReviewId.From(id));
builder.Property(p => p.Rating).HasConversion(r => r.Value, r => ReviewRating.From(r));
builder.Property(p => p.Comment).HasConversion(c => c.Value, c => ReviewComment.From(c)).HasMaxLength(ReviewComment.MaxLength);
builder.OptimisticConcurrencyVersionMap();

// Read configuration
builder.HasKey(x => x.Id);
```

### 4.3 Repository Implementasyonu
```csharp
// YASAK — endpoint'ten direkt DbContext kullanmak
// DOĞRU — Repository pattern

// Command: context inject eder, ProductReview.Create() çağırır
internal sealed class EfProductReviewCommandRepository(ApplicationWriteDbContext context)
    : IProductReviewCommandRepository { ... }

// Query: context inject eder, read model döner
internal sealed class EfProductReviewQueryRepository(ApplicationReadDbContext context)
    : IProductReviewQueryRepository { ... }
```

### 4.4 Soft Delete
- `IsDeleted = true` olan kayıtlar query filter ile otomatik gizlenir
- `ApplySoftDeleteQueryFilter()` her iki context'e de uygulanır

### 4.5 Audit Fields
- `CreatedAt` ve `UpdatedAt` EF Core interceptor (`AuditableInterceptor`) ile otomatik doldurulur
- Hardcode etme, interceptor'a bırak

### 4.6 Migration Script
```bash
# Dosya: WebApi klasöründe migration.sh
# Çalıştırma: ./migration.sh InitialProductReview
dotnet ef migrations add $MIGRATION_NAME \
  --project ../MuhammedTask.Services.ProductReviewService.Persistence \
  --context ApplicationWriteDbContext \
  --output-dir EntityFrameworkCore/Migrations/ApplicationWrite
```

### 4.7 Integration Event
```csharp
// MassTransit IPublisher ile domain event handler'dan yayınlanır
// Payload: ProductId, NewAverageRating, ReviewCount
namespace MuhammedTask.IntegrationEvents.ProductReviews;
public sealed record ProductReviewCreatedIntegrationEvent(
    Guid ProductId,
    double NewAverageRating,
    int ReviewCount);
```

---

## 5. WebApi Layer Kuralları

### 5.1 Carter Module Pattern
```csharp
public sealed class ProductReviewCreateEndpoint : CarterModule
{
    public sealed record ProductReviewCreateRequest(Guid ProductId, Guid UserId, int? Rating, string? Comment)
    {
        public CreateProductReviewCommand ToCommand() => new(ProductId, UserId, Rating, Comment);
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

    private static Task<IResult> CreateProductReview(
        [FromBody] ProductReviewCreateRequest request,
        ISender sender,
        CancellationToken cancellationToken = default) =>
        sender
            .Send(request.ToCommand(), cancellationToken)
            .Match(
                id => TypedResults.Created($"/{Tags.ProductReviews}/{id.Value}", id.Value),
                errors => errors.ToProblemResult(),
                cancellationToken);
}
```

### 5.2 Endpoint Route'ları
```
POST   /api/v1/productreviews
GET    /api/v1/productreviews/{id}
GET    /api/v1/products/{productId}/reviews
GET    /api/v1/products/{productId}/average-rating
PATCH  /api/v1/productreviews/{id}
DELETE /api/v1/productreviews/{id}
```

### 5.3 Result → ProblemDetails
- Tüm endpoint'ler `errors.ToProblemResult()` kullanır
- Endpoint içinde direkt exception fırlatmak **yasak**

### 5.4 Hardcoded Değer Yasağı
```csharp
// YASAK
.AddKeycloakJwtBearer(keycloakServiceId: "keycloak", realm: "productreviews")
// DOĞRU — ServiceKeys sabitinden oku
.AddKeycloakJwtBearer(keycloakServiceId: ServiceKeys.Keycloak, realm: "productreviews")
```

---

## 6. AppHost Kuralları

```csharp
// WaitFor pattern — postgres ve rabbitmq hazır olmadan servis başlamasın
IResourceBuilder<ProjectResource> productReviewService = builder
    .AddProject<Projects.MuhammedTask_Services_ProductReviewService_WebApi>(productReviewServiceKey)
    .WithHttpHealthCheck("/health")
    .WithReference(seq)
    .WithReference(cache)
    .WithReference(rabbitmq)
    .WithReference(productReviewServicePostgresDatabase)
    .WaitFor(productReviewServicePostgresDatabase)  // zorunlu
    .WaitFor(rabbitmq)                              // zorunlu
    .WithReference(keycloak)
    .WaitFor(keycloak);
```

---

## 7. Commit Kuralları

- **Tek devasa commit yasak** — her aşama ayrı commit
- Commit mesajı formatı: `feat(productreview): ...` / `test(productreview): ...` / `docs: ...`
- Her commit çalışır durumda olacak (build kırmayacak)
- Migration ayrı commit olacak

---

## 8. Anti-Pattern Özeti (Task.md'den)

| Anti-Pattern | Sonuç |
|---|---|
| `throw new Exception(...)` | Puan kırma |
| `string userId` primitive obsession | Puan kırma |
| Handler'da business logic | Puan kırma |
| `.Result` / `.Wait()` async'te | Puan kırma |
| Endpoint'ten direkt DbContext | Puan kırma |
| Hardcoded değerler | Puan kırma |
| Domain event'i handler dışında publish | Puan kırma |
| Migration commit'i atlamak | Puan kırma |
| Tek büyük commit | Puan kırma |
