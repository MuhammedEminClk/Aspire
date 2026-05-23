1. Genel Bilgi
Bu görev, Medarvia ekibine katılmak için başvuran Yazılımcı Yardımcısı (Part Time) adaylarının kod
yazma stilini, mimari yetkinliğini ve karar verme yaklaşımını değerlendirmek üzere hazırlanmıştır.
Aşağıdaki gereksinimleri yerine getirerek bize çalışma stilinizi gösterin; bizim de doğru kararı
verebilmemiz için yeterli bağlamı sunun.

• Tahmini efor: 4–6 saat
• Teslim süresi: Görevi aldığınız andan itibaren 7 gün
• Teslim biçimi: Public GitHub fork üzerinde PR açın; PR linkini iletişim adresine gönderin
• Hedef: Clean Architecture + CQRS + Domain-Driven Design pattern'lerinde nasıl kod yazdığınızı
ve mimari kararlar verirken neyi önemsediğinizi görmek
2. Başlangıç Adımları
Aşağıdaki repo, görevin temel iskeletini oluşturuyor. Aynı pattern'leri taklit ederek yeni servisi
ekleyeceksiniz:

# 1. Repo'yu fork edin
https://github.com/senrecep/Aspire
# 2. Template'i kurun
dotnet new install SenRecep.Aspire
# 3. Yeni proje yaratın (kendi adınızla)
mkdir candidate-task && cd candidate-task
dotnet new aspire-microservice-starter -n CandidateTask
# 4. Aspire host ile çalıştırın
cd source/src/Host/CandidateTask.AppHost
dotnet run

Aspire dashboard'da Postgres, Redis, RabbitMQ, Seq ve Keycloak container'larının ayağa kalktığını
doğrulayın. ProductService ve CategoryService endpoint'lerinin Swagger üzerinden eriştiğinizi test
edin. Yeni servisinizi yazmadan önce bu iki servisin nasıl yapılandırıldığını dikkatle inceleyin.

Hatırlatma:
Mevcut ProductService'in pattern'lerini birebir taklit edin. Yeni bir mimari, kütüphane veya kalıp icat
etmeyin. Amaç ekibe uyumlu kod yazabildiğinizi göstermek; özgün tasarım değerlendirilmez.

3. Görev — ProductReview Microservice
Yeni bir mikroservis ekleyeceksiniz: ProductReview. Kullanıcılar bir ürünü 1–5 yıldız ile
değerlendirebilir, isteğe bağlı bir yorum ekleyebilir. Aşağıdaki katman gereksinimlerini eksiksiz
uygulayın.
3.1 Domain Layer
• ProductReview aggregate root: Id, ProductId, UserId, Rating, Comment, CreatedAt, UpdatedAt,
IsDeleted (soft delete)
• Value Objects:
◦ ProductReviewId (StronglyTypedId<Guid>)
◦ ProductId (StronglyTypedId<Guid>) — referans alınan dış aggregate
◦ UserId (StronglyTypedId<Guid>)
◦ ReviewRating — 1–5 aralığını enforce eden factory method
◦ ReviewComment — opsiyonel, max 1000 karakter
• Parameters: ProductReviewCreateParameters, ProductReviewUpdateParameters
• ReadModel: ProductReviewReadModel (query projection)
• Repository arayüzleri: IProductReviewCommandRepository, IProductReviewQueryRepository
• Domain Event'ler: ProductReviewCreatedDomainEvent, ProductReviewUpdatedDomainEvent,
ProductReviewDeletedDomainEvent
• Errors: ProductReviewErrors static sınıfı (NotFound, AlreadyReviewed, InvalidRating)
• Factory method: ProductReview.Create() — Result<ProductReview> döner, tüm validation
içerir
• Update method: ProductReview.Update() — Result döner
3.2 Application Layer
Commands
• CreateProductReviewCommand + Handler + Validator (FluentValidation)
• UpdateProductReviewCommand + Handler + Validator
• DeleteProductReviewCommand + Handler
Queries
• GetProductReviewByIdQuery + Handler
• GetProductReviewListQuery — paginated, ProductId ile filtrelenebilir
• GetProductAverageRatingQuery — tek bir ürünün ortalama puanını döner
Domain Event Handler
• ProductReviewCreatedDomainEventHandler — ilgili ürünün ortalama puan cache key'ini
invalidate eder

İş Kuralları
• Aynı kullanıcı aynı ProductId için ikinci bir değerlendirme yazamaz (AlreadyReviewed error
döner)
• Bir kullanıcı yalnızca kendi değerlendirmesini update veya delete edebilir (UserId match check)
3.3 IntegrationEvent Layer
• ProductReviewCreatedIntegrationEvent — MassTransit üzerinden RabbitMQ'ya yayınlanır
• Payload: ProductId, NewAverageRating, ReviewCount
Bu event'i kim tüketecek diye düşünmenize gerek yok; yayın tarafı yeterli.
3.4 Persistence Layer
• ApplicationWriteDbContext ve ApplicationReadDbContext (CQRS ayrımı)
• EF Core konfigürasyonları: Configurations/Read ve Configurations/Write klasörleri
• EfProductReviewCommandRepository, EfProductReviewQueryRepository
• Initial migration: ProductService'in migration.sh script pattern'ini taklit ederek üretin
• Soft delete query filter — IsDeleted = true olan kayıtlar query sonuçlarında yer almasın
• Audit fields (CreatedAt, UpdatedAt) bir EF Core interceptor ile otomatik doldurulsun
3.5 WebApi Layer
Carter modülleri ile RESTful endpoint'ler yazın:

POST /api/v1/productreviews
GET /api/v1/productreviews/{id}
GET /api/v1/products/{productId}/reviews (paginated)
GET /api/v1/products/{productId}/average-rating
PATCH /api/v1/productreviews/{id}
DELETE /api/v1/productreviews/{id}

• Result pattern → ProblemDetails dönüşümü
• Swagger UI'da görünür ve test edilebilir olsun
• API versioning (/v1/ prefix)
• YARP Proxy üzerinden routing eklenmiş olsun
3.6 Aspire AppHost
• ProductReview servisini AppHost'a builder.AddProject ile ekleyin
• Kendi Postgres database'i: pg-productreviewservice
• RabbitMQ ve Redis WithReference ile bağlayın
• WaitFor pattern'i doğru kullanın (postgres ve rabbitmq hazır olmadan servis ayağa kalkmamalı)

4. Teslim Kontrol Listesi
PR açmadan önce aşağıdaki maddelerin tamamını doğrulayın:
☐ Fork ettiğin GitHub repo URL'i PR description'da var
☐ dotnet run ile Aspire dashboard hatasız başlıyor
☐ Tüm endpoint'ler Swagger üzerinden manuel test edildi
☐ Migration script ile DB schema'sı yaratılıyor: ./migration.sh InitialProductReview
☐ dotnet test komutu en az 3 unit test ile yeşil dönüyor
☐ README'de tasarım kararlarını açıklayan kısa bir bölüm var
☐ PR açıklaması: yaklaşımını ve trade-off'larını anlatan 1–2 paragraf içeriyor

6. Anti-pattern'ler
Aşağıdaki uygulamalar otomatik olarak puan kıracaktır. Bunlardan kaçının:
• throw new Exception(...) kullanımı (Result pattern var, exception fırlatmayın)
• Primitive obsession: string userId yerine UserId value object kullanın
• Anemic domain: business logic'i handler'a yazıp entity'i salt veri konteyneri yapmak
• Async method içinde .Result veya .Wait() çağırmak (deadlock riski)
• Endpoint içinde doğrudan DbContext kullanmak (Repository pattern'i bypass etmek)
• Hardcoded değerler — config'den okunmalı
• Domain event'i handler dışında IPublisher ile publish etmek
• Migration commit'i atlama veya migration.sh pattern'i dışına çıkmak
• Tek devasa commit — anlamlı, küçük commit'ler bekleniyor
7. Bonus (zorunlu değil, ekstra puan)
• WebApplicationFactory ile en az 1 endpoint için integration test
• GetProductAverageRatingQuery için Redis caching uygulaması (cache invalidate domain event
handler ile)
• XML doc comments — Swagger üzerinden endpoint açıklamaları okunabilir olsun
• Pagination response wrapper'ı (PageNumber, PageSize, TotalCount, Items)
