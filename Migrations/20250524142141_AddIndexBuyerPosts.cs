using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexBuyerPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyerPosts_Categorys_CategoryId",
                table: "BuyerPosts");

            migrationBuilder.CreateIndex(
                name: "IX_BuyerPosts_CreatedAt_CategoryId",
                table: "BuyerPosts",
                columns: new[] { "CreatedAt", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BuyerPosts_Categorys_CategoryId",
                table: "BuyerPosts",
                column: "CategoryId",
                principalTable: "Categorys",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyerPosts_Categorys_CategoryId",
                table: "BuyerPosts");

            migrationBuilder.DropIndex(
                name: "IX_BuyerPosts_CreatedAt_CategoryId",
                table: "BuyerPosts");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyerPosts_Categorys_CategoryId",
                table: "BuyerPosts",
                column: "CategoryId",
                principalTable: "Categorys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
