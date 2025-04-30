using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddItemsQueryFilterAndChatRoomField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ChatRooms",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ChatRooms");
        }
    }
}
