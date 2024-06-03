using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRecieverFieldInChatMeesageFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chat_messages_chat_members_ChatMemberId",
                table: "chat_messages");

            migrationBuilder.DropIndex(
                name: "IX_chat_messages_ChatMemberId",
                table: "chat_messages");

            migrationBuilder.DropColumn(
                name: "ChatMemberId",
                table: "chat_messages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChatMemberId",
                table: "chat_messages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_ChatMemberId",
                table: "chat_messages",
                column: "ChatMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_chat_messages_chat_members_ChatMemberId",
                table: "chat_messages",
                column: "ChatMemberId",
                principalTable: "chat_members",
                principalColumn: "id");
        }
    }
}
