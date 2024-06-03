using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class OwnerNameChangeAndAddChats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_aveton_role_accesses_owner_id",
                table: "aveton_role_accesses");

            migrationBuilder.DropForeignKey(
                name: "fk_aveton_roles_owner_id",
                table: "aveton_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_aveton_users_owner_id",
                table: "aveton_users");

            migrationBuilder.DropForeignKey(
                name: "fk_aveton_users_roles_owner_id",
                table: "aveton_users_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_clients_owner_id",
                table: "clients");

            migrationBuilder.DropForeignKey(
                name: "fk_division_contractors_owner_id",
                table: "division_contractors");

            migrationBuilder.DropForeignKey(
                name: "fk_divisions_owner_id",
                table: "divisions");

            migrationBuilder.DropForeignKey(
                name: "fk_employees_owner_id",
                table: "employees");

            migrationBuilder.DropForeignKey(
                name: "fk_jobs_owner_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_organizations_owner_id",
                table: "organizations");

            migrationBuilder.DropForeignKey(
                name: "fk_persons_owner_id",
                table: "persons");

            migrationBuilder.DropForeignKey(
                name: "fk_positions_owner_id",
                table: "positions");

            migrationBuilder.DropForeignKey(
                name: "fk_project_stages_owner_id",
                table: "project_stages");

            migrationBuilder.DropForeignKey(
                name: "fk_projects_owner_id",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "fk_stage_managers_owner_id",
                table: "stage_managers");

            migrationBuilder.DropForeignKey(
                name: "fk_stage_report_attached_files_owner_id",
                table: "stage_report_attached_files");

            migrationBuilder.DropForeignKey(
                name: "fk_stage_reports_owner_id",
                table: "stage_reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Owners",
                table: "Owners");

            migrationBuilder.RenameTable(
                name: "Owners",
                newName: "system_owners");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "stage_reports",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_stage_reports_owner_id",
                table: "stage_reports",
                newName: "IX_stage_reports_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "stage_report_attached_files",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_stage_report_attached_files_owner_id",
                table: "stage_report_attached_files",
                newName: "IX_stage_report_attached_files_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "stage_managers",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_stage_managers_owner_id",
                table: "stage_managers",
                newName: "IX_stage_managers_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "projects",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_projects_owner_id",
                table: "projects",
                newName: "IX_projects_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "project_stages",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_project_stages_owner_id",
                table: "project_stages",
                newName: "IX_project_stages_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "positions",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_positions_owner_id",
                table: "positions",
                newName: "IX_positions_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "persons",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_persons_owner_id",
                table: "persons",
                newName: "IX_persons_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "organizations",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_organizations_owner_id",
                table: "organizations",
                newName: "IX_organizations_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "jobs",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_jobs_owner_id",
                table: "jobs",
                newName: "IX_jobs_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "employees",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_employees_owner_id",
                table: "employees",
                newName: "IX_employees_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "divisions",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_divisions_owner_id",
                table: "divisions",
                newName: "IX_divisions_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "division_contractors",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_division_contractors_owner_id",
                table: "division_contractors",
                newName: "IX_division_contractors_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "clients",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_clients_owner_id",
                table: "clients",
                newName: "IX_clients_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "aveton_users_roles",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_aveton_users_roles_owner_id",
                table: "aveton_users_roles",
                newName: "IX_aveton_users_roles_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "aveton_users",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_aveton_users_owner_id",
                table: "aveton_users",
                newName: "IX_aveton_users_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "aveton_roles",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_aveton_roles_owner_id",
                table: "aveton_roles",
                newName: "IX_aveton_roles_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "aveton_role_accesses",
                newName: "entity_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_aveton_role_accesses_owner_id",
                table: "aveton_role_accesses",
                newName: "IX_aveton_role_accesses_entity_owner_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "system_owners",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "system_owners",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "system_owners",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_system_owners",
                table: "system_owners",
                column: "id");

            migrationBuilder.CreateTable(
                name: "chats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    path_to_avatar_image = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_group_chat = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    entity_owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chats", x => x.id);
                    table.ForeignKey(
                        name: "fk_chats_entity_owner_id",
                        column: x => x.entity_owner_id,
                        principalTable: "system_owners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chat_members",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    person_id = table.Column<Guid>(type: "uuid", nullable: true),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: true),
                    entity_owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_members", x => x.id);
                    table.ForeignKey(
                        name: "fk_chat_members_chat_id",
                        column: x => x.chat_id,
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_chat_members_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_chat_members_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_chat_members_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_projects_entity_owner_id",
                        column: x => x.entity_owner_id,
                        principalTable: "system_owners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chat_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reciever_id = table.Column<Guid>(type: "uuid", nullable: true),
                    text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    is_viewed = table.Column<bool>(type: "boolean", nullable: true),
                    view_datetime = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: true),
                    entity_owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_chat_messages_chat_id",
                        column: x => x.chat_id,
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_chat_messages_owner_id",
                        column: x => x.owner_id,
                        principalTable: "chat_members",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_chat_messages_reciever_id",
                        column: x => x.chat_id,
                        principalTable: "chat_members",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_projects_entity_owner_id",
                        column: x => x.entity_owner_id,
                        principalTable: "system_owners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chat_message_attached_files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    file_path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    entity_owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_message_attached_files", x => x.id);
                    table.ForeignKey(
                        name: "fk_chat_message_attached_files_chat_id",
                        column: x => x.chat_id,
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_chat_message_attached_files_message_id",
                        column: x => x.message_id,
                        principalTable: "chat_messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_projects_entity_owner_id",
                        column: x => x.entity_owner_id,
                        principalTable: "system_owners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chat_message_viewed_infos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    viewed_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_message_viewed_infos", x => x.id);
                    table.ForeignKey(
                        name: "fk_chat_message_viewed_infos_message_id",
                        column: x => x.message_id,
                        principalTable: "chat_messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_chat_message_viewed_infos_viewed_by_id",
                        column: x => x.viewed_by_id,
                        principalTable: "chat_members",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_projects_entity_owner_id",
                        column: x => x.entity_owner_id,
                        principalTable: "system_owners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_system_owners_id",
                table: "system_owners",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_members_chat_id",
                table: "chat_members",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_members_employee_id",
                table: "chat_members",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_members_entity_owner_id",
                table: "chat_members",
                column: "entity_owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_members_id",
                table: "chat_members",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_members_organization_id",
                table: "chat_members",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_members_person_id",
                table: "chat_members",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_message_attached_files_chat_id",
                table: "chat_message_attached_files",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_message_attached_files_entity_owner_id",
                table: "chat_message_attached_files",
                column: "entity_owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_message_attached_files_id",
                table: "chat_message_attached_files",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_message_attached_files_message_id",
                table: "chat_message_attached_files",
                column: "message_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_message_viewed_infos_entity_owner_id",
                table: "chat_message_viewed_infos",
                column: "entity_owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_message_viewed_infos_id",
                table: "chat_message_viewed_infos",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_message_viewed_infos_message_id",
                table: "chat_message_viewed_infos",
                column: "message_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_message_viewed_infos_viewed_by_id",
                table: "chat_message_viewed_infos",
                column: "viewed_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_chat_id",
                table: "chat_messages",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_entity_owner_id",
                table: "chat_messages",
                column: "entity_owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_id",
                table: "chat_messages",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_owner_id",
                table: "chat_messages",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_reciever_id",
                table: "chat_messages",
                column: "reciever_id");

            migrationBuilder.CreateIndex(
                name: "IX_chats_entity_owner_id",
                table: "chats",
                column: "entity_owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_chats_id",
                table: "chats",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_aveton_role_accesses_entity_owner_id",
                table: "aveton_role_accesses",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_aveton_roles_entity_owner_id",
                table: "aveton_roles",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_aveton_users_entity_owner_id",
                table: "aveton_users",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_aveton_users_roles_entity_owner_id",
                table: "aveton_users_roles",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_clients_entity_owner_id",
                table: "clients",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_division_contractors_entity_owner_id",
                table: "division_contractors",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_divisions_entity_owner_id",
                table: "divisions",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_employees_entity_owner_id",
                table: "employees",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_entity_owner_id",
                table: "jobs",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_organizations_entity_owner_id",
                table: "organizations",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_persons_entity_owner_id",
                table: "persons",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_positions_entity_owner_id",
                table: "positions",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_project_stages_entity_owner_id",
                table: "project_stages",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_projects_entity_owner_id",
                table: "projects",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_stage_managers_entity_owner_id",
                table: "stage_managers",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_stage_report_attached_files_entity_owner_id",
                table: "stage_report_attached_files",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_stage_reports_entity_owner_id",
                table: "stage_reports",
                column: "entity_owner_id",
                principalTable: "system_owners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_aveton_role_accesses_entity_owner_id",
                table: "aveton_role_accesses");

            migrationBuilder.DropForeignKey(
                name: "fk_aveton_roles_entity_owner_id",
                table: "aveton_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_aveton_users_entity_owner_id",
                table: "aveton_users");

            migrationBuilder.DropForeignKey(
                name: "fk_aveton_users_roles_entity_owner_id",
                table: "aveton_users_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_clients_entity_owner_id",
                table: "clients");

            migrationBuilder.DropForeignKey(
                name: "fk_division_contractors_entity_owner_id",
                table: "division_contractors");

            migrationBuilder.DropForeignKey(
                name: "fk_divisions_entity_owner_id",
                table: "divisions");

            migrationBuilder.DropForeignKey(
                name: "fk_employees_entity_owner_id",
                table: "employees");

            migrationBuilder.DropForeignKey(
                name: "fk_jobs_entity_owner_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_organizations_entity_owner_id",
                table: "organizations");

            migrationBuilder.DropForeignKey(
                name: "fk_persons_entity_owner_id",
                table: "persons");

            migrationBuilder.DropForeignKey(
                name: "fk_positions_entity_owner_id",
                table: "positions");

            migrationBuilder.DropForeignKey(
                name: "fk_project_stages_entity_owner_id",
                table: "project_stages");

            migrationBuilder.DropForeignKey(
                name: "fk_projects_entity_owner_id",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "fk_stage_managers_entity_owner_id",
                table: "stage_managers");

            migrationBuilder.DropForeignKey(
                name: "fk_stage_report_attached_files_entity_owner_id",
                table: "stage_report_attached_files");

            migrationBuilder.DropForeignKey(
                name: "fk_stage_reports_entity_owner_id",
                table: "stage_reports");

            migrationBuilder.DropTable(
                name: "chat_message_attached_files");

            migrationBuilder.DropTable(
                name: "chat_message_viewed_infos");

            migrationBuilder.DropTable(
                name: "chat_messages");

            migrationBuilder.DropTable(
                name: "chat_members");

            migrationBuilder.DropTable(
                name: "chats");

            migrationBuilder.DropPrimaryKey(
                name: "pk_system_owners",
                table: "system_owners");

            migrationBuilder.DropIndex(
                name: "IX_system_owners_id",
                table: "system_owners");

            migrationBuilder.RenameTable(
                name: "system_owners",
                newName: "Owners");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "stage_reports",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_stage_reports_entity_owner_id",
                table: "stage_reports",
                newName: "IX_stage_reports_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "stage_report_attached_files",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_stage_report_attached_files_entity_owner_id",
                table: "stage_report_attached_files",
                newName: "IX_stage_report_attached_files_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "stage_managers",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_stage_managers_entity_owner_id",
                table: "stage_managers",
                newName: "IX_stage_managers_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "projects",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_projects_entity_owner_id",
                table: "projects",
                newName: "IX_projects_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "project_stages",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_project_stages_entity_owner_id",
                table: "project_stages",
                newName: "IX_project_stages_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "positions",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_positions_entity_owner_id",
                table: "positions",
                newName: "IX_positions_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "persons",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_persons_entity_owner_id",
                table: "persons",
                newName: "IX_persons_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "organizations",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_organizations_entity_owner_id",
                table: "organizations",
                newName: "IX_organizations_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "jobs",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_jobs_entity_owner_id",
                table: "jobs",
                newName: "IX_jobs_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "employees",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_employees_entity_owner_id",
                table: "employees",
                newName: "IX_employees_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "divisions",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_divisions_entity_owner_id",
                table: "divisions",
                newName: "IX_divisions_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "division_contractors",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_division_contractors_entity_owner_id",
                table: "division_contractors",
                newName: "IX_division_contractors_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "clients",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_clients_entity_owner_id",
                table: "clients",
                newName: "IX_clients_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "aveton_users_roles",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_aveton_users_roles_entity_owner_id",
                table: "aveton_users_roles",
                newName: "IX_aveton_users_roles_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "aveton_users",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_aveton_users_entity_owner_id",
                table: "aveton_users",
                newName: "IX_aveton_users_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "aveton_roles",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_aveton_roles_entity_owner_id",
                table: "aveton_roles",
                newName: "IX_aveton_roles_owner_id");

            migrationBuilder.RenameColumn(
                name: "entity_owner_id",
                table: "aveton_role_accesses",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_aveton_role_accesses_entity_owner_id",
                table: "aveton_role_accesses",
                newName: "IX_aveton_role_accesses_owner_id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Owners",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Owners",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Owners",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Owners",
                table: "Owners",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "fk_aveton_role_accesses_owner_id",
                table: "aveton_role_accesses",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_aveton_roles_owner_id",
                table: "aveton_roles",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_aveton_users_owner_id",
                table: "aveton_users",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_aveton_users_roles_owner_id",
                table: "aveton_users_roles",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_clients_owner_id",
                table: "clients",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_division_contractors_owner_id",
                table: "division_contractors",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_divisions_owner_id",
                table: "divisions",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_employees_owner_id",
                table: "employees",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_owner_id",
                table: "jobs",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_organizations_owner_id",
                table: "organizations",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_persons_owner_id",
                table: "persons",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_positions_owner_id",
                table: "positions",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_project_stages_owner_id",
                table: "project_stages",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_projects_owner_id",
                table: "projects",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_stage_managers_owner_id",
                table: "stage_managers",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_stage_report_attached_files_owner_id",
                table: "stage_report_attached_files",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_stage_reports_owner_id",
                table: "stage_reports",
                column: "owner_id",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
