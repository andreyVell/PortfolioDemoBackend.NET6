using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class ClientNewUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_clients_organization_id",
                table: "clients");

            migrationBuilder.DropIndex(
                name: "IX_clients_person_id",
                table: "clients");

            migrationBuilder.DropIndex(
                name: "IX_clients_project_id_person_id_organization_id",
                table: "clients");

            migrationBuilder.CreateIndex(
                name: "IX_clients_organization_id",
                table: "clients",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_clients_person_id",
                table: "clients",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_clients_project_id_person_id_organization_id",
                table: "clients",
                columns: new[] { "project_id", "person_id", "organization_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_clients_organization_id",
                table: "clients");

            migrationBuilder.DropIndex(
                name: "IX_clients_person_id",
                table: "clients");

            migrationBuilder.DropIndex(
                name: "IX_clients_project_id_person_id_organization_id",
                table: "clients");

            migrationBuilder.CreateIndex(
                name: "IX_clients_organization_id",
                table: "clients",
                column: "organization_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clients_person_id",
                table: "clients",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clients_project_id_person_id_organization_id",
                table: "clients",
                columns: new[] { "project_id", "person_id", "organization_id" });
        }
    }
}
