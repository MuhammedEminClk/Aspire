# Aspire Microservice Starter

[![NuGet](https://img.shields.io/nuget/v/SenRecep.Aspire.svg)](https://www.nuget.org/packages/SenRecep.Aspire)
[![NuGet Downloads](https://img.shields.io/nuget/dt/SenRecep.Aspire.svg)](https://www.nuget.org/packages/SenRecep.Aspire)
[![GitHub](https://img.shields.io/github/license/SenRecep/Aspire)](https://github.com/SenRecep/Aspire)
[![GitHub Stars](https://img.shields.io/github/stars/SenRecep/Aspire)](https://github.com/SenRecep/Aspire)

A modern, production-ready microservices template built with .NET Aspire 9.0. This template provides a robust foundation for building scalable, maintainable, and cloud-ready microservices applications.

## Features

- **.NET Aspire Integration**: Built on .NET 9.0 with full Aspire support for cloud-native development
- **Microservices Architecture**:
  - Product Service
  - Category Service
  - API Gateway (YARP Proxy)
- **Building Blocks**:
  - Domain-Driven Design (DDD) structure
  - Clean Architecture implementation
  - Shared infrastructure components
- **Infrastructure Components**:
  - PostgreSQL database integration
  - Redis caching
  - RabbitMQ message broker
  - Seq logging
  - Keycloak authentication
- **Development Features**:
  - Centralized package management
  - Code analysis and quality tools
  - Entity Framework Core with PostgreSQL
  - MassTransit for message handling
  - OpenTelemetry integration

## Project Structure

```
source/
├── src/
│   ├── BuildingBlocks/           # Shared components and utilities
│   │   ├── Caching/             # Redis and base caching implementations
│   │   ├── Database/            # Database access and migrations
│   │   ├── Logging/            # Serilog implementation
│   │   ├── MessageBrokers/     # MassTransit and message broker implementations
│   │   └── OpenTelemetry/      # Telemetry and monitoring
│   ├── Services/                # Microservices
│   │   ├── ProductService/     # Product management service
│   │   ├── CategoryService/    # Category management service
│   │   └── ProxyService/      # YARP API Gateway
│   └── Host/                    # Aspire host and configuration
└── tests/                       # Test projects
```

## Prerequisites

- .NET 9.0 SDK
- Docker Desktop
- Visual Studio 2022 or later (recommended)

## Installation

1. Install the template:

```bash
dotnet new install SenRecep.Aspire
```

2. Create a new project:

```bash
mkdir YourProjectName
cd YourProjectName
dotnet new aspire-microservice-starter -n YourProjectName
```

## Getting Started

1. Clone the repository
2. Navigate to the solution directory
3. Run the application:

```bash
cd source/src/Host/YourProjectName.AppHost
dotnet run
```

## Development Environment

The template uses .NET Aspire's orchestration to manage:

- PostgreSQL databases for each service
- Redis cache
- RabbitMQ message broker
- Seq for centralized logging
- Keycloak for authentication

## Architecture

This template follows Clean Architecture principles with a domain-driven design approach:

- **Domain Layer**: Core business logic and entities
- **Application Layer**: Use cases and business rules
- **Infrastructure Layer**: External concerns and implementations
- **Presentation Layer**: API endpoints and controllers

## Configuration

The template uses centralized package management through `Directory.Packages.props` and shared build properties in `Directory.Build.props`.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🔍 Related Projects

### CSharpEssentials

[CSharpEssentials](https://github.com/SenRecep/CSharpEssentials) is a comprehensive library that enhances C#'s functional programming capabilities. It provides a robust set of tools and utilities designed to make your C# applications more maintainable, testable, and aligned with functional programming principles.

Key features:

- Functional Programming Essentials (Maybe Monad, Result Pattern, Discriminated Unions)
- Domain Modeling Tools (Rule Engine, Error Types, Entity Base)
- ASP.NET Core Integration
- Entity Framework Core Integration
- Request/Response Logging
- Enhanced JSON capabilities
- Time Management utilities

This library complements the Aspire Microservice Starter by providing additional functional programming patterns and utilities that can be used to build more robust and maintainable microservices.

---

## ProductReviewService — Design Decisions

This section documents the key design decisions made while implementing the `ProductReviewService` as part of the take-home task. The service follows the same patterns as `ProductService` and `CategoryService`.

### Domain Layer

**`ReviewComment` as a nullable value object (`ReviewComment?`)**

`ReviewComment` is a `readonly record struct`. Because structs cannot be `null` in C#, making the EF column optional required declaring the domain property as `ReviewComment?` (nullable struct) rather than `ReviewComment`. This allowed using an explicit `ValueConverter<ReviewComment?, string?>` in the EF configuration, which correctly handles `null` values in both directions.

**Business rules enforced inside the aggregate**

`ProductReview.Update()` and `ProductReview.Delete()` both receive the calling `UserId` and check it against the stored `UserId` before proceeding. If they don't match, they return `ProductReviewErrors.UnauthorizedError` immediately — no service-layer authorization check needed. This keeps ownership logic inside the domain where it belongs.

**Domain events raised on every state change**

`ProductReviewCreatedDomainEvent`, `ProductReviewUpdatedDomainEvent`, and `ProductReviewDeletedDomainEvent` are raised via `Raise()` inside the aggregate methods. They are published by the `PublishDomainEventsInterceptor` after `SaveChangesAsync`, keeping the domain free of any `IPublisher` dependency.

### Application Layer

**AlreadyReviewed check in the command handler**

The `CreateProductReviewCommandHandler` calls `IProductReviewQueryRepository.ExistsAsync(productId, userId)` before attempting to create. If a review already exists, it returns `ProductReviewErrors.AlreadyReviewedError`. This is a pre-condition check — the uniqueness is also enforced at the database level (see below) as a safety net.

**Cache invalidation via domain event handler**

`ProductReviewCreatedDomainEventHandler` invalidates the `"reviews"` cache tag so that list and average-rating queries reflect the new state. This decouples cache invalidation from the command handler and follows the same pattern used by `ProductService`.

**Three separate cached queries**

- `GetProductReviewByIdQuery` — cached by `"productreview:{id}"`
- `GetProductReviewListQuery` — cached with tags `["reviews", "product:{productId}"]`
- `GetProductAverageRatingQuery` — cached by `"product:{productId}:average-rating"`

Separate cache keys allow fine-grained invalidation without clearing unrelated entries.

### Persistence Layer

**Write/Read DbContext separation**

`ApplicationWriteDbContext` (tracking, domain events interceptor, auditable interceptor) and `ApplicationReadDbContext` (no-tracking, interceptors disabled) are registered as separate pooled DbContexts pointing to the same PostgreSQL database. This enforces the CQRS boundary at the infrastructure level.

**Unique index as a database-level guard for AlreadyReviewed**

A unique composite index on `(product_id, user_id)` is created in the migration. Even if the application-level check in the command handler were bypassed under concurrent writes, the database would reject the duplicate with a unique constraint violation, which `EntityFrameworkCore.Exceptions.PostgreSQL` maps to a conflict error.

**`IDesignTimeDbContextFactory` for EF CLI**

The `AddPooledPostgresDbContext` helper in `BuildingBlocks` (which applies `UseSnakeCaseNamingConvention()`) is not available at design time. A dedicated `ApplicationWriteDbContextFactory` reads the connection string from an environment variable and registers only the minimal services needed for EF CLI commands (`dotnet ef migrations add`, `dotnet ef database update`).

**`PendingModelChangesWarning` suppressed**

`UseSnakeCaseNamingConvention()` is applied at runtime by the BuildingBlocks helper but is not reflected in the EF model snapshot (which is generated by the design-time factory without it). This causes EF to report a model/snapshot mismatch warning on every startup. The warning is suppressed via `ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))` and the migration is written manually with the correct snake_case column and index names.

### WebApi Layer

**No inter-service HTTP clients**

`ProductReviewService` does not call any other service over HTTP, so there is no `ConfigureHttpClients()` registration — unlike `ProductService`, which references `CategoryService`. All data it needs is self-contained within its own database.

**Six endpoints via Carter modules**

| Method | Route | Purpose |
|--------|-------|---------|
| POST | `/api/v1/productreviews` | Create a review |
| PATCH | `/api/v1/productreviews/{reviewId}` | Update a review |
| DELETE | `/api/v1/productreviews/{reviewId}` | Delete a review |
| GET | `/api/v1/productreviews/{reviewId}` | Get a single review |
| GET | `/api/v1/products/{productId}/reviews` | List reviews for a product |
| GET | `/api/v1/products/{productId}/average-rating` | Get average rating |
