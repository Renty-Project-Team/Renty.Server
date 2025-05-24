using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Server.Migrations
{
    /// <inheritdoc />
    public partial class CreateNewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyerPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BuyerUserId = table.Column<string>(type: "TEXT", nullable: false, comment: "차용인 id"),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, comment: "제목"),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false, comment: "내용"),
                    ViewCount = table.Column<int>(type: "INTEGER", nullable: false, comment: "조회수"),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "등록 시간"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "수정 시간"),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true, comment: "삭제 시간")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyerPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyerPosts_AspNetUsers_BuyerUserId",
                        column: x => x.BuyerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RevieweeId = table.Column<string>(type: "TEXT", nullable: false, comment: "판매자 id"),
                    ReviewerId = table.Column<string>(type: "TEXT", nullable: false, comment: "구매자 id"),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false, comment: "아이템 id"),
                    Satisfaction = table.Column<float>(type: "REAL", nullable: false, comment: "만족도 (5점 만점)"),
                    Content = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false, comment: "리뷰 내용")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_RevieweeId",
                        column: x => x.RevieweeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuyerComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false, comment: "유저 id"),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false, comment: "제안 아이템 id"),
                    BuyerPostId = table.Column<int>(type: "INTEGER", nullable: false, comment: "댓글 포스트 id"),
                    Content = table.Column<string>(type: "TEXT", nullable: true, comment: "내용"),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "등록 시간"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "수정 시간")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyerComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyerComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyerComments_BuyerPosts_BuyerPostId",
                        column: x => x.BuyerPostId,
                        principalTable: "BuyerPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyerComments_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReviewId = table.Column<int>(type: "INTEGER", nullable: false, comment: "리뷰 id"),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false, comment: "이미지 경로"),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false, comment: "이미지 순서"),
                    UploadedAt = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "업로드 시간")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewImages_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuyerComments_BuyerPostId",
                table: "BuyerComments",
                column: "BuyerPostId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyerComments_ItemId",
                table: "BuyerComments",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyerComments_UserId",
                table: "BuyerComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyerPosts_BuyerUserId",
                table: "BuyerPosts",
                column: "BuyerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewImages_ReviewId",
                table: "ReviewImages",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ItemId",
                table: "Reviews",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RevieweeId",
                table: "Reviews",
                column: "RevieweeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewerId",
                table: "Reviews",
                column: "ReviewerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyerComments");

            migrationBuilder.DropTable(
                name: "ReviewImages");

            migrationBuilder.DropTable(
                name: "BuyerPosts");

            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
