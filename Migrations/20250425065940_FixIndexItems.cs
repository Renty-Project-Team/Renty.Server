using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Server.Migrations
{
    /// <inheritdoc />
    public partial class FixIndexItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Items_UpdatedAt_Id",
                table: "Items");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CreatedAt_Id",
                table: "Items",
                columns: new[] { "CreatedAt", "Id" },
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Items_CreatedAt_Id",
                table: "Items");

            migrationBuilder.CreateIndex(
                name: "IX_Items_UpdatedAt_Id",
                table: "Items",
                columns: new[] { "UpdatedAt", "Id" },
                descending: new bool[0]);
        }
    }
}
