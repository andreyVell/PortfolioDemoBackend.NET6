using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class relationLinksChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_clients_organization_id",
                table: "clients");

            migrationBuilder.DropForeignKey(
                name: "fk_clients_person_id",
                table: "clients");

            migrationBuilder.DropForeignKey(
                name: "fk_divisions_parent_division_id",
                table: "divisions");

            migrationBuilder.DropForeignKey(
                name: "fk_employees_aveton_user_id",
                table: "employees");

            migrationBuilder.DropForeignKey(
                name: "fk_jobs_division_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_jobs_employee_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_jobs_position_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_project_stages_parent_stage_id",
                table: "project_stages");

            migrationBuilder.DropForeignKey(
                name: "fk_projects_manager_id",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_stage_managers_employees_employee_id",
                table: "stage_managers");

            migrationBuilder.DropForeignKey(
                name: "FK_stage_report_attached_files_stage_reports_stage_report_id",
                table: "stage_report_attached_files");

            migrationBuilder.DropForeignKey(
                name: "FK_stage_reports_employees_employee_id",
                table: "stage_reports");

            migrationBuilder.DropForeignKey(
                name: "FK_stage_reports_project_stages_project_stage_id",
                table: "stage_reports");

            migrationBuilder.DropForeignKey(
                name: "FK_stage_reports_stage_managers_stage_manager_id",
                table: "stage_reports");

            migrationBuilder.AddForeignKey(
                name: "fk_clients_organization_id",
                table: "clients",
                column: "organization_id",
                principalTable: "organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_clients_person_id",
                table: "clients",
                column: "person_id",
                principalTable: "persons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_division_id",
                table: "jobs",
                column: "division_id",
                principalTable: "divisions",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_employee_id",
                table: "jobs",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_position_id",
                table: "jobs",
                column: "position_id",
                principalTable: "positions",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_project_stages_parent_stage_id",
                table: "project_stages",
                column: "parent_stage_id",
                principalTable: "project_stages",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_projects_manager_id",
                table: "projects",
                column: "manager_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_stage_managers_employees_employee_id",
                table: "stage_managers",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_stage_report_attached_files_stage_reports_stage_report_id",
                table: "stage_report_attached_files",
                column: "stage_report_id",
                principalTable: "stage_reports",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_stage_reports_employees_employee_id",
                table: "stage_reports",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_stage_reports_project_stages_project_stage_id",
                table: "stage_reports",
                column: "project_stage_id",
                principalTable: "project_stages",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stage_reports_stage_managers_stage_manager_id",
                table: "stage_reports",
                column: "stage_manager_id",
                principalTable: "stage_managers",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_clients_organization_id",
                table: "clients");

            migrationBuilder.DropForeignKey(
                name: "fk_clients_person_id",
                table: "clients");

            migrationBuilder.DropForeignKey(
                name: "fk_divisions_parent_division_id",
                table: "divisions");

            migrationBuilder.DropForeignKey(
                name: "fk_employees_aveton_user_id",
                table: "employees");

            migrationBuilder.DropForeignKey(
                name: "fk_jobs_division_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_jobs_employee_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_jobs_position_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_project_stages_parent_stage_id",
                table: "project_stages");

            migrationBuilder.DropForeignKey(
                name: "fk_projects_manager_id",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_stage_managers_employees_employee_id",
                table: "stage_managers");

            migrationBuilder.DropForeignKey(
                name: "FK_stage_report_attached_files_stage_reports_stage_report_id",
                table: "stage_report_attached_files");

            migrationBuilder.DropForeignKey(
                name: "FK_stage_reports_employees_employee_id",
                table: "stage_reports");

            migrationBuilder.DropForeignKey(
                name: "FK_stage_reports_project_stages_project_stage_id",
                table: "stage_reports");

            migrationBuilder.DropForeignKey(
                name: "FK_stage_reports_stage_managers_stage_manager_id",
                table: "stage_reports");

            migrationBuilder.AddForeignKey(
                name: "fk_clients_organization_id",
                table: "clients",
                column: "organization_id",
                principalTable: "organizations",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_clients_person_id",
                table: "clients",
                column: "person_id",
                principalTable: "persons",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_divisions_parent_division_id",
                table: "divisions",
                column: "parent_division_id",
                principalTable: "divisions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_employees_aveton_user_id",
                table: "employees",
                column: "credentials_id",
                principalTable: "aveton_users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_division_id",
                table: "jobs",
                column: "division_id",
                principalTable: "divisions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_employee_id",
                table: "jobs",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_position_id",
                table: "jobs",
                column: "position_id",
                principalTable: "positions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_project_stages_parent_stage_id",
                table: "project_stages",
                column: "parent_stage_id",
                principalTable: "project_stages",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_projects_manager_id",
                table: "projects",
                column: "manager_id",
                principalTable: "employees",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_stage_managers_employees_employee_id",
                table: "stage_managers",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stage_report_attached_files_stage_reports_stage_report_id",
                table: "stage_report_attached_files",
                column: "stage_report_id",
                principalTable: "stage_reports",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_stage_reports_employees_employee_id",
                table: "stage_reports",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_stage_reports_project_stages_project_stage_id",
                table: "stage_reports",
                column: "project_stage_id",
                principalTable: "project_stages",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_stage_reports_stage_managers_stage_manager_id",
                table: "stage_reports",
                column: "stage_manager_id",
                principalTable: "stage_managers",
                principalColumn: "id");
        }
    }
}
