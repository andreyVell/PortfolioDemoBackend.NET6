using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteOnProjectStageDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stage_managers_employees_employee_id",
                table: "stage_managers");

            migrationBuilder.DropForeignKey(
                name: "FK_stage_managers_project_stages_project_stage_id",
                table: "stage_managers");

            migrationBuilder.AddForeignKey(
                name: "FK_stage_managers_employees_employee_id",
                table: "stage_managers",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stage_managers_project_stages_project_stage_id",
                table: "stage_managers",
                column: "project_stage_id",
                principalTable: "project_stages",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stage_managers_employees_employee_id",
                table: "stage_managers");

            migrationBuilder.DropForeignKey(
                name: "FK_stage_managers_project_stages_project_stage_id",
                table: "stage_managers");

            migrationBuilder.AddForeignKey(
                name: "FK_stage_managers_employees_employee_id",
                table: "stage_managers",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_stage_managers_project_stages_project_stage_id",
                table: "stage_managers",
                column: "project_stage_id",
                principalTable: "project_stages",
                principalColumn: "id");
        }
    }
}
