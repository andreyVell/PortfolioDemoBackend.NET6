using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class fixChatMessageRecieverIdRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chat_messages_reciever_id",
                table: "chat_messages");

            migrationBuilder.DropForeignKey(
                name: "FK_employees_aveton_users_AvetonUserId",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "IX_employees_AvetonUserId",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "AvetonUserId",
                table: "employees");

            migrationBuilder.AddForeignKey(
                name: "fk_chat_messages_reciever_id",
                table: "chat_messages",
                column: "reciever_id",
                principalTable: "chat_members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chat_messages_reciever_id",
                table: "chat_messages");

            migrationBuilder.AddColumn<Guid>(
                name: "AvetonUserId",
                table: "employees",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_AvetonUserId",
                table: "employees",
                column: "AvetonUserId");

            migrationBuilder.AddForeignKey(
                name: "fk_chat_messages_reciever_id",
                table: "chat_messages",
                column: "chat_id",
                principalTable: "chat_members",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employees_aveton_users_AvetonUserId",
                table: "employees",
                column: "AvetonUserId",
                principalTable: "aveton_users",
                principalColumn: "id");
        }
    }
}
