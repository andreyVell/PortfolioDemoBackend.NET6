using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class ChatMessageAttachedFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "chat_messages");

            migrationBuilder.DropColumn(
                name: "type",
                table: "chat_message_attached_files");

            migrationBuilder.AddColumn<string>(
                name: "image_medium_size_file_path",
                table: "chat_message_attached_files",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_medium_size_file_path",
                table: "chat_message_attached_files");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "chat_messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "chat_message_attached_files",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
