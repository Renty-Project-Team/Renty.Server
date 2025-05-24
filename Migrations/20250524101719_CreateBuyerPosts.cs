using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Server.Migrations
{
    /// <inheritdoc />
    public partial class CreateBuyerPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "BuyerPosts",
                type: "TEXT",
                nullable: true,
                comment: "수정 시간",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldComment: "수정 시간");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "BuyerPosts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                comment: "카테고리 id");

            migrationBuilder.CreateTable(
                name: "BuyerPostImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BuyerPostId = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyerPostImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyerPostImages_BuyerPosts_BuyerPostId",
                        column: x => x.BuyerPostId,
                        principalTable: "BuyerPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuyerPosts_CategoryId",
                table: "BuyerPosts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyerPostImages_BuyerPostId",
                table: "BuyerPostImages",
                column: "BuyerPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyerPosts_Categorys_CategoryId",
                table: "BuyerPosts",
                column: "CategoryId",
                principalTable: "Categorys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyerPosts_Categorys_CategoryId",
                table: "BuyerPosts");

            migrationBuilder.DropTable(
                name: "BuyerPostImages");

            migrationBuilder.DropIndex(
                name: "IX_BuyerPosts_CategoryId",
                table: "BuyerPosts");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "BuyerPosts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "BuyerPosts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "수정 시간",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldComment: "수정 시간");
        }
    }
}
