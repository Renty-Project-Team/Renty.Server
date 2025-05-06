using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFieldNameTradeOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TradeOffers_ItemId",
                table: "TradeOffers");

            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "TradeOffers");

            migrationBuilder.RenameColumn(
                name: "UnitOfTime",
                table: "TradeOffers",
                newName: "PriceUnit");

            migrationBuilder.CreateIndex(
                name: "IX_TradeOffers_ItemId_BuyerId",
                table: "TradeOffers",
                columns: new[] { "ItemId", "BuyerId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TradeOffers_ItemId_BuyerId",
                table: "TradeOffers");

            migrationBuilder.RenameColumn(
                name: "PriceUnit",
                table: "TradeOffers",
                newName: "UnitOfTime");

            migrationBuilder.AddColumn<decimal>(
                name: "FinalPrice",
                table: "TradeOffers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_TradeOffers_ItemId",
                table: "TradeOffers",
                column: "ItemId");
        }
    }
}
