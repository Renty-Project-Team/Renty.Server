using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renty.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeChatRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoomName",
                table: "ChatRooms",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomName",
                table: "ChatRooms");
        }
    }
}
