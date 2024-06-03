using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class AddChatBigAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "path_to_avatar_image",
                table: "chats",
                newName: "path_to_avatar_small_image");

            migrationBuilder.AddColumn<string>(
                name: "path_to_avatar_big_image",
                table: "chats",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "path_to_avatar_big_image",
                table: "chats");

            migrationBuilder.RenameColumn(
                name: "path_to_avatar_small_image",
                table: "chats",
                newName: "path_to_avatar_image");
        }
    }
}
