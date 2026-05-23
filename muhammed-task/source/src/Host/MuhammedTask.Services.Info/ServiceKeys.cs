namespace MuhammedTask.Services.Info;

public static class ServiceKeys
{
    public const string Yarp = "yarp";
    public const string Seq = "seq";
    public const string PostgresServer = "postgres-server";
    public const string Redis = "redis";
    public const string RabbitMQ = "rabbitmq";

    public const string Keycloak = "keycloak";
    public const string ProductService = "productservice";
    public const string CategoryService = "categoryservice";
    public const string ProductReviewService = "productreviewservice";

    public static class Database
    {
        public const string PostgresProductService = "pg-productservice";
        public const string PostgresCategoryService = "pg-categoryservice";
        public const string PostgresProductReviewService = "pg-productreviewservice";
    }
}
