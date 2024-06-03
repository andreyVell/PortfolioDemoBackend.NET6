using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class addMessageTextIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_text",
                table: "chat_messages",
                column: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_chat_messages_text",
                table: "chat_messages");
        }
    }
}
