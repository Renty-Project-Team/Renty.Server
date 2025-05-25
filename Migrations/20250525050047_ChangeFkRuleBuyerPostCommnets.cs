using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFkRuleBuyerPostCommnets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyerComments_Items_ItemId",
                table: "BuyerComments");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "BuyerComments",
                type: "INTEGER",
                nullable: true,
                comment: "제안 아이템 id",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "제안 아이템 id");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyerComments_Items_ItemId",
                table: "BuyerComments",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyerComments_Items_ItemId",
                table: "BuyerComments");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "BuyerComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                comment: "제안 아이템 id",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true,
                oldComment: "제안 아이템 id");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyerComments_Items_ItemId",
                table: "BuyerComments",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
