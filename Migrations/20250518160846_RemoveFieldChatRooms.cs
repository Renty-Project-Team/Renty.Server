using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFieldChatRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ChatRooms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ChatRooms",
                type: "TEXT",
                nullable: true);
        }
    }
}
