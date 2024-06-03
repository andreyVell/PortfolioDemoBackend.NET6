using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRecieverFieldInChatMeesage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chat_messages_reciever_id",
                table: "chat_messages");

            migrationBuilder.RenameColumn(
                name: "reciever_id",
                table: "chat_messages",
                newName: "ChatMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_chat_messages_reciever_id",
                table: "chat_messages",
                newName: "IX_chat_messages_ChatMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_chat_messages_chat_members_ChatMemberId",
                table: "chat_messages",
                column: "ChatMemberId",
                principalTable: "chat_members",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chat_messages_chat_members_ChatMemberId",
                table: "chat_messages");

            migrationBuilder.RenameColumn(
                name: "ChatMemberId",
                table: "chat_messages",
                newName: "reciever_id");

            migrationBuilder.RenameIndex(
                name: "IX_chat_messages_ChatMemberId",
                table: "chat_messages",
                newName: "IX_chat_messages_reciever_id");

            migrationBuilder.AddForeignKey(
                name: "fk_chat_messages_reciever_id",
                table: "chat_messages",
                column: "reciever_id",
                principalTable: "chat_members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
