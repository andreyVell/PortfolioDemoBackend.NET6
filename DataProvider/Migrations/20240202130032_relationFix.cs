using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class relationFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_divisions_parent_division_id",
                table: "divisions");

            migrationBuilder.DropForeignKey(
                name: "fk_employees_aveton_user_id",
                table: "employees");

            migrationBuilder.AddForeignKey(
                name: "fk_divisions_parent_division_id",
                table: "divisions",
                column: "parent_division_id",
                principalTable: "divisions",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_employees_aveton_user_id",
                table: "employees",
                column: "credentials_id",
                principalTable: "aveton_users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_divisions_parent_division_id",
                table: "divisions");

            migrationBuilder.DropForeignKey(
                name: "fk_employees_aveton_user_id",
                table: "employees");

            migrationBuilder.AddForeignKey(
                name: "fk_divisions_parent_division_id",
                table: "divisions",
                column: "parent_division_id",
                principalTable: "divisions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_employees_aveton_user_id",
                table: "employees",
                column: "credentials_id",
                principalTable: "aveton_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
