using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class SetNullOnChatMessageOwnerDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chat_messages_owner_id",
                table: "chat_messages");

            migrationBuilder.AddForeignKey(
                name: "fk_chat_messages_owner_id",
                table: "chat_messages",
                column: "owner_id",
                principalTable: "chat_members",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chat_messages_owner_id",
                table: "chat_messages");

            migrationBuilder.AddForeignKey(
                name: "fk_chat_messages_owner_id",
                table: "chat_messages",
                column: "owner_id",
                principalTable: "chat_members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
