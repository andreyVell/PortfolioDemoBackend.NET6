using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class TransformChatMessagesViewInfoStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_viewed",
                table: "chat_messages");

            migrationBuilder.DropColumn(
                name: "view_datetime",
                table: "chat_messages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_viewed",
                table: "chat_messages",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "view_datetime",
                table: "chat_messages",
                type: "timestamp(6) with time zone",
                nullable: true);
        }
    }
}
