# ProductReview Microservice — Roadmap

Task.md'e göre yapılacaklar sıralaması. Her aşama tamamlandığında commit atılacak.
Namespace: `MuhammedTask.Services.ProductReviewService.*`

---

## Aşama 0 — Hazırlık
- [x] Task.md okundu ve analiz edildi
- [x] ProductService ve CategoryService pattern'leri incelendi
- [x] ROADMAP.md ve RULES.md oluşturuldu

**Commit:** `docs: add ROADMAP.md and RULES.md`

---

## Aşama 1 — Proje İskeleti
- [ ] `MuhammedTask.Services.ProductReviewService.Domain` csproj
- [ ] `MuhammedTask.Services.ProductReviewService.Application` csproj
- [ ] `MuhammedTask.Services.ProductReviewService.Persistence` csproj
- [ ] `MuhammedTask.Services.ProductReviewService.WebApi` csproj
- [ ] Solution'a ekleme (`MuhammedTask.sln`)
- [ ] `ServiceKeys.cs`'e `ProductReviewService` ve `pg-productreviewservice` ekleme

**Commit:** `feat(productreview): add project skeleton and solution references`

---

## Aşama 2 — Domain Layer
Dosya yolu: `source/src/Services/ProductReviewService/MuhammedTask.Services.ProductReviewService.Domain/`

### 2.1 Value Objects (Fields/)
- [ ] `ProductReviewId` — readonly record struct, `New()`, `From()`, `Empty`
- [ ] `ProductId` — readonly record struct (dış aggregate referansı)
- [ ] `UserId` — readonly record struct
- [ ] `ReviewRating` — 1–5 enforce, `Create()` → `Result<ReviewRating>`
- [ ] `ReviewComment` — opsiyonel, max 1000 karakter, `Create()` → `Result<ReviewComment>`

### 2.2 Parameters
- [ ] `ProductReviewCreateParameters` — sealed record
- [ ] `ProductReviewUpdateParameters` — sealed record

### 2.3 Errors
- [ ] `ProductReviewErrors` static sınıfı — NotFound, AlreadyReviewed, nested Rating ve Comment hataları

### 2.4 Domain Events (Events/)
- [ ] `ProductReviewCreatedDomainEvent`
- [ ] `ProductReviewUpdatedDomainEvent`
- [ ] `ProductReviewDeletedDomainEvent`

### 2.5 Repository Interfaces (Repositories/)
- [ ] `IProductReviewCommandRepository`
- [ ] `IProductReviewQueryRepository`

### 2.6 ReadModel
- [ ] `ProductReviewReadModel` — ISoftDeletableBase, primitive tipler

### 2.7 Aggregate Root
- [ ] `ProductReview` — SoftDeletableEntityBase<ProductReviewId>
  - `Create()` → `Result<ProductReview>` (validation dahil)
  - `Update()` → `Result`
  - `Delete()` → domain event raise
  - Business rule: aynı user aynı product'a 2. review yapamaz

### 2.8 Assembly Reference
- [ ] `DomainAssemblyReference.cs`

**Commit:** `feat(productreview): implement domain layer — entities, value objects, events, errors`

---

## Aşama 3 — Application Layer
Dosya yolu: `source/src/Services/ProductReviewService/MuhammedTask.Services.ProductReviewService.Application/`

### 3.1 Commands
- [ ] `CreateProductReviewCommand` + `Handler` + `Validator`
- [ ] `UpdateProductReviewCommand` + `Handler` + `Validator`
- [ ] `DeleteProductReviewCommand` + `Handler`

### 3.2 Queries
- [ ] `GetProductReviewByIdQuery` + `Handler` + `Validator`
- [ ] `GetProductReviewListQuery` (ProductId filtreli, paginated) + `Handler` + `Validator`
- [ ] `GetProductAverageRatingQuery` + `Handler`

### 3.3 View Models
- [ ] `ProductReviewViewModel` — readonly record struct
- [ ] `ProductAverageRatingViewModel` — readonly record struct

### 3.4 Domain Event Handler
- [ ] `ProductReviewCreatedDomainEventHandler` — cache invalidate

### 3.5 DI Registration
- [ ] `ApplicationAssemblyReference.cs`
- [ ] `ServiceRegistrations/DependencyInjection.cs`

**Commit:** `feat(productreview): implement application layer — commands, queries, event handlers`

---

## Aşama 4 — Persistence Layer
Dosya yolu: `source/src/Services/ProductReviewService/MuhammedTask.Services.ProductReviewService.Persistence/`

### 4.1 DbContexts
- [ ] `ApplicationWriteDbContext` — WriteDbContextBase, ApplySoftDeleteQueryFilter
- [ ] `ApplicationReadDbContext` — ReadDbContextBase, NoTracking, ApplySoftDeleteQueryFilter

### 4.2 EF Configurations
- [ ] `Write/ProductReviewConfiguration` — SoftDeletableEntityBaseMap, HasConversion, OptimisticConcurrencyVersionMap
- [ ] `Read/ProductReviewReadModelConfiguration` — HasKey

### 4.3 Repositories
- [ ] `EfProductReviewCommandRepository` — Create, Update, Delete
- [ ] `EfProductReviewQueryRepository` — GetById (Maybe<T>), GetList (array), GetAverageRating

### 4.4 Integration Event
- [ ] `ProductReviewCreatedIntegrationEvent` — Payload: ProductId, NewAverageRating, ReviewCount
- [ ] MassTransit publisher (domain event handler'dan tetiklenir)

### 4.5 DI Registration
- [ ] `PersistenceAssemblyReference.cs`
- [ ] `ServiceRegistrations/DependencyInjection.cs`
- [ ] `ServiceRegistrations/RepositoryRegistrations.cs`
- [ ] `ServiceRegistrations/ServicesRegistrations.cs`

**Commit:** `feat(productreview): implement persistence layer — dbcontext, ef configs, repositories`

---

## Aşama 5 — WebApi Layer
Dosya yolu: `source/src/Services/ProductReviewService/MuhammedTask.Services.ProductReviewService.WebApi/`

### 5.1 Carter Endpoints
- [ ] `POST /api/v1/productreviews` — CreateEndpoint
- [ ] `GET /api/v1/productreviews/{id}` — GetEndpoint
- [ ] `GET /api/v1/products/{productId}/reviews` — ListEndpoint (paginated)
- [ ] `GET /api/v1/products/{productId}/average-rating` — AverageRatingEndpoint
- [ ] `PATCH /api/v1/productreviews/{id}` — UpdateEndpoint
- [ ] `DELETE /api/v1/productreviews/{id}` — DeleteEndpoint

### 5.2 Altyapı
- [ ] `Tags.cs`
- [ ] `Program.cs`
- [ ] `ServiceRegistrations/DependencyInjection.cs`
- [ ] `ServiceRegistrations/OpenTelemetryDependencyInjection.cs`
- [ ] `migration.sh` — ProductService pattern'i ile aynı

**Commit:** `feat(productreview): implement webapi layer — carter endpoints, swagger, versioning`

---

## Aşama 6 — AppHost & YARP Entegrasyonu

- [ ] `ServiceKeys.cs` güncelleme — `ProductReviewService`, `PostgresProductReviewService`
- [ ] `AppHost/Program.cs` — builder.AddProject, WaitFor pattern
- [ ] `Yarp.ProxyService/appsettings.json` — productreviews route ve cluster

**Commit:** `feat(productreview): wire up apphost and yarp proxy routing`

---

## Aşama 7 — Migration

- [ ] `./migration.sh InitialProductReview` çalıştır
- [ ] Migration dosyalarını kontrol et

**Commit:** `feat(productreview): add initial EF Core migration`

---

## Aşama 8 — Unit Tests

- [ ] Domain value object testleri (ReviewRating, ReviewComment)
- [ ] Domain aggregate root testleri (Create, Update, Delete)
- [ ] En az 3 test — `dotnet test` yeşil

**Commit:** `test(productreview): add unit tests for domain layer`

---

## Aşama 9 — README Güncellemesi

- [ ] README.md'e tasarım kararları bölümü ekle

**Commit:** `docs: update README with design decisions`

---

## Bonus (Zorunlu Değil)

- [ ] Redis caching — `GetProductAverageRatingQuery` için
- [ ] Cache invalidation domain event handler'dan
- [ ] XML doc comments — Swagger açıklamaları
- [ ] Pagination wrapper — PageNumber, PageSize, TotalCount, Items
- [ ] Integration test — WebApplicationFactory

**Commit:** `feat(productreview): add bonus features — caching, pagination, xml docs`

---

## Teslim Kontrol Listesi (PR Öncesi)

- [ ] Fork edilmiş GitHub repo URL'i PR description'da
- [ ] `dotnet run` → Aspire dashboard hatasız başlıyor
- [ ] Tüm endpoint'ler Swagger'dan test edildi
- [ ] `./migration.sh InitialProductReview` çalışıyor
- [ ] `dotnet test` en az 3 test ile yeşil dönüyor
- [ ] README'de tasarım kararları bölümü var
- [ ] PR açıklaması: yaklaşım ve trade-off'lar
