using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuhammedTask.Services.ProductReviewService.Persistence.EntityFrameworkCore.Migrations.ApplicationWrite;

/// <inheritdoc />
public partial class InitialProductReview : Migration
{
    private static readonly string[] CreatedAtIsDeletedColumns = ["created_at", "is_deleted"];
    private static readonly string[] ProductIdUserIdColumns = ["product_id", "user_id"];

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "product_reviews",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                product_id = table.Column<Guid>(type: "uuid", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                rating = table.Column<int>(type: "integer", nullable: false),
                comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                created_by = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                updated_by = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                deleted_by = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table => table.PrimaryKey("pk_product_reviews", x => x.id));

        migrationBuilder.CreateIndex(
            name: "ix_product_reviews_created_at_is_deleted",
            table: "product_reviews",
            columns: CreatedAtIsDeletedColumns);

        migrationBuilder.CreateIndex(
            name: "ix_product_reviews_product_id",
            table: "product_reviews",
            column: "product_id");

        migrationBuilder.CreateIndex(
            name: "ix_product_reviews_product_id_user_id",
            table: "product_reviews",
            columns: ProductIdUserIdColumns,
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.DropTable(name: "product_reviews");
}
