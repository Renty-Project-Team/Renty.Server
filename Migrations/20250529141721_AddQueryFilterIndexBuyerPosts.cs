using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddQueryFilterIndexBuyerPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BuyerPosts_CreatedAt_CategoryId",
                table: "BuyerPosts");

            migrationBuilder.CreateIndex(
                name: "IX_BuyerPosts_DeletedAt_BuyerUserId",
                table: "BuyerPosts",
                columns: new[] { "DeletedAt", "BuyerUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_BuyerPosts_DeletedAt_CreatedAt_CategoryId",
                table: "BuyerPosts",
                columns: new[] { "DeletedAt", "CreatedAt", "CategoryId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BuyerPosts_DeletedAt_BuyerUserId",
                table: "BuyerPosts");

            migrationBuilder.DropIndex(
                name: "IX_BuyerPosts_DeletedAt_CreatedAt_CategoryId",
                table: "BuyerPosts");

            migrationBuilder.CreateIndex(
                name: "IX_BuyerPosts_CreatedAt_CategoryId",
                table: "BuyerPosts",
                columns: new[] { "CreatedAt", "CategoryId" });
        }
    }
}
