using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexWishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WishLists_UserId",
                table: "WishLists");

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_UserId_ItemId",
                table: "WishLists",
                columns: new[] { "UserId", "ItemId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WishLists_UserId_ItemId",
                table: "WishLists");

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_UserId",
                table: "WishLists",
                column: "UserId");
        }
    }
}
