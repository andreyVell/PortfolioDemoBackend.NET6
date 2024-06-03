using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class TrialInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "aveton_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: true),
                    is_system_administrator = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aveton_roles", x => x.id);
                    table.ForeignKey(
                        name: "fk_aveton_roles_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aveton_users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password_salt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aveton_users", x => x.id);
                    table.UniqueConstraint("AK_aveton_users_login", x => x.login);
                    table.ForeignKey(
                        name: "fk_aveton_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "divisions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    parent_division_id = table.Column<Guid>(type: "uuid", nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_divisions", x => x.id);
                    table.ForeignKey(
                        name: "fk_divisions_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_divisions_parent_division_id",
                        column: x => x.parent_division_id,
                        principalTable: "divisions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "organizations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    inn = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    contact_email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    contact_phone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizations", x => x.id);
                    table.ForeignKey(
                        name: "fk_organizations_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "persons",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    last_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    second_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    contact_email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    contact_phone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_persons", x => x.id);
                    table.ForeignKey(
                        name: "fk_persons_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "positions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_positions", x => x.id);
                    table.ForeignKey(
                        name: "fk_positions_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aveton_role_accesses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    entity_action = table.Column<int>(type: "integer", nullable: false),
                    is_allowed = table.Column<bool>(type: "boolean", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aveton_role_accesses", x => x.id);
                    table.ForeignKey(
                        name: "fk_aveton_role_accesses_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_aveton_role_accesses_role_id",
                        column: x => x.role_id,
                        principalTable: "aveton_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aveton_users_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aveton_users_roles", x => x.id);
                    table.ForeignKey(
                        name: "FK_aveton_users_roles_aveton_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "aveton_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_aveton_users_roles_aveton_users_user_id",
                        column: x => x.user_id,
                        principalTable: "aveton_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_aveton_users_roles_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    last_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    second_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    mobile_phone_number = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    birthday = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: true),
                    path_to_avatar = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    credentials_id = table.Column<Guid>(type: "uuid", nullable: true),
                    AvetonUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees", x => x.id);
                    table.ForeignKey(
                        name: "FK_employees_aveton_users_AvetonUserId",
                        column: x => x.AvetonUserId,
                        principalTable: "aveton_users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_employees_aveton_user_id",
                        column: x => x.credentials_id,
                        principalTable: "aveton_users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_employees_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "jobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: true),
                    position_id = table.Column<Guid>(type: "uuid", nullable: true),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    division_id = table.Column<Guid>(type: "uuid", nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_jobs", x => x.id);
                    table.ForeignKey(
                        name: "fk_jobs_division_id",
                        column: x => x.division_id,
                        principalTable: "divisions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_jobs_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_jobs_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_jobs_position_id",
                        column: x => x.position_id,
                        principalTable: "positions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    manager_id = table.Column<Guid>(type: "uuid", nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projects", x => x.id);
                    table.ForeignKey(
                        name: "fk_projects_manager_id",
                        column: x => x.manager_id,
                        principalTable: "employees",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_projects_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    client_type = table.Column<int>(type: "integer", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    person_id = table.Column<Guid>(type: "uuid", nullable: true),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clients", x => x.id);
                    table.ForeignKey(
                        name: "fk_clients_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_clients_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_clients_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_clients_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project_stages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_stage_id = table.Column<Guid>(type: "uuid", nullable: true),
                    order_number = table.Column<int>(type: "integer", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_stages", x => x.id);
                    table.ForeignKey(
                        name: "fk_project_stages_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_project_stages_parent_stage_id",
                        column: x => x.parent_stage_id,
                        principalTable: "project_stages",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_project_stages_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "division_contractors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_stage_id = table.Column<Guid>(type: "uuid", nullable: false),
                    division_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_division_contractors", x => x.id);
                    table.ForeignKey(
                        name: "FK_division_contractors_divisions_division_id",
                        column: x => x.division_id,
                        principalTable: "divisions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_division_contractors_project_stages_project_stage_id",
                        column: x => x.project_stage_id,
                        principalTable: "project_stages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_division_contractors_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stage_managers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_stage_id = table.Column<Guid>(type: "uuid", nullable: true),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stage_managers", x => x.id);
                    table.ForeignKey(
                        name: "FK_stage_managers_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stage_managers_project_stages_project_stage_id",
                        column: x => x.project_stage_id,
                        principalTable: "project_stages",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_stage_managers_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stage_reports",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    report_date = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    project_stage_id = table.Column<Guid>(type: "uuid", nullable: true),
                    stage_manager_id = table.Column<Guid>(type: "uuid", nullable: true),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stage_reports", x => x.id);
                    table.ForeignKey(
                        name: "FK_stage_reports_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stage_reports_project_stages_project_stage_id",
                        column: x => x.project_stage_id,
                        principalTable: "project_stages",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_stage_reports_stage_managers_stage_manager_id",
                        column: x => x.stage_manager_id,
                        principalTable: "stage_managers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_stage_reports_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stage_report_attached_files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    file_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    stage_report_id = table.Column<Guid>(type: "uuid", nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_by_user_login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false),
                    updated_on = table.Column<DateTime>(type: "timestamp(6) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stage_report_attached_files", x => x.id);
                    table.ForeignKey(
                        name: "FK_stage_report_attached_files_stage_reports_stage_report_id",
                        column: x => x.stage_report_id,
                        principalTable: "stage_reports",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_stage_report_attached_files_owner_id",
                        column: x => x.owner_id,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_aveton_role_accesses_entity_action",
                table: "aveton_role_accesses",
                column: "entity_action");

            migrationBuilder.CreateIndex(
                name: "IX_aveton_role_accesses_entity_name",
                table: "aveton_role_accesses",
                column: "entity_name");

            migrationBuilder.CreateIndex(
                name: "IX_aveton_role_accesses_id",
                table: "aveton_role_accesses",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_aveton_role_accesses_is_allowed",
                table: "aveton_role_accesses",
                column: "is_allowed");

            migrationBuilder.CreateIndex(
                name: "IX_aveton_role_accesses_owner_id",
                table: "aveton_role_accesses",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_aveton_role_accesses_role_id",
                table: "aveton_role_accesses",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_aveton_roles_id",
                table: "aveton_roles",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_aveton_roles_owner_id",
                table: "aveton_roles",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_aveton_users_id",
                table: "aveton_users",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_aveton_users_login",
                table: "aveton_users",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_aveton_users_owner_id",
                table: "aveton_users",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_aveton_users_roles_id",
                table: "aveton_users_roles",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_aveton_users_roles_owner_id",
                table: "aveton_users_roles",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_aveton_users_roles_role_id",
                table: "aveton_users_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_aveton_users_roles_user_id",
                table: "aveton_users_roles",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_clients_id",
                table: "clients",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_clients_organization_id",
                table: "clients",
                column: "organization_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clients_owner_id",
                table: "clients",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_clients_person_id",
                table: "clients",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clients_project_id_person_id_organization_id",
                table: "clients",
                columns: new[] { "project_id", "person_id", "organization_id" });

            migrationBuilder.CreateIndex(
                name: "IX_division_contractors_division_id_project_stage_id",
                table: "division_contractors",
                columns: new[] { "division_id", "project_stage_id" });

            migrationBuilder.CreateIndex(
                name: "IX_division_contractors_id",
                table: "division_contractors",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_division_contractors_owner_id",
                table: "division_contractors",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_division_contractors_project_stage_id",
                table: "division_contractors",
                column: "project_stage_id");

            migrationBuilder.CreateIndex(
                name: "IX_divisions_id",
                table: "divisions",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_divisions_owner_id",
                table: "divisions",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_divisions_parent_division_id",
                table: "divisions",
                column: "parent_division_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_AvetonUserId",
                table: "employees",
                column: "AvetonUserId");

            migrationBuilder.CreateIndex(
                name: "IX_employees_credentials_id",
                table: "employees",
                column: "credentials_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_id",
                table: "employees",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_owner_id",
                table: "employees",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_division_id",
                table: "jobs",
                column: "division_id");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_employee_id",
                table: "jobs",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_id",
                table: "jobs",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_owner_id",
                table: "jobs",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_position_id",
                table: "jobs",
                column: "position_id");

            migrationBuilder.CreateIndex(
                name: "IX_organizations_id",
                table: "organizations",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_organizations_owner_id",
                table: "organizations",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_persons_id",
                table: "persons",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_persons_owner_id",
                table: "persons",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_positions_id",
                table: "positions",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_positions_owner_id",
                table: "positions",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_stages_id",
                table: "project_stages",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_project_stages_owner_id",
                table: "project_stages",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_stages_parent_stage_id",
                table: "project_stages",
                column: "parent_stage_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_stages_project_id",
                table: "project_stages",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_projects_id",
                table: "projects",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_projects_manager_id",
                table: "projects",
                column: "manager_id");

            migrationBuilder.CreateIndex(
                name: "IX_projects_owner_id",
                table: "projects",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_stage_managers_employee_id",
                table: "stage_managers",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_stage_managers_id",
                table: "stage_managers",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_stage_managers_owner_id",
                table: "stage_managers",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_stage_managers_project_stage_id_employee_id",
                table: "stage_managers",
                columns: new[] { "project_stage_id", "employee_id" });

            migrationBuilder.CreateIndex(
                name: "IX_stage_report_attached_files_id",
                table: "stage_report_attached_files",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_stage_report_attached_files_owner_id",
                table: "stage_report_attached_files",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_stage_report_attached_files_stage_report_id",
                table: "stage_report_attached_files",
                column: "stage_report_id");

            migrationBuilder.CreateIndex(
                name: "IX_stage_reports_employee_id",
                table: "stage_reports",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_stage_reports_id",
                table: "stage_reports",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_stage_reports_owner_id",
                table: "stage_reports",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_stage_reports_project_stage_id",
                table: "stage_reports",
                column: "project_stage_id");

            migrationBuilder.CreateIndex(
                name: "IX_stage_reports_stage_manager_id",
                table: "stage_reports",
                column: "stage_manager_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aveton_role_accesses");

            migrationBuilder.DropTable(
                name: "aveton_users_roles");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "division_contractors");

            migrationBuilder.DropTable(
                name: "jobs");

            migrationBuilder.DropTable(
                name: "stage_report_attached_files");

            migrationBuilder.DropTable(
                name: "aveton_roles");

            migrationBuilder.DropTable(
                name: "organizations");

            migrationBuilder.DropTable(
                name: "persons");

            migrationBuilder.DropTable(
                name: "divisions");

            migrationBuilder.DropTable(
                name: "positions");

            migrationBuilder.DropTable(
                name: "stage_reports");

            migrationBuilder.DropTable(
                name: "stage_managers");

            migrationBuilder.DropTable(
                name: "project_stages");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "aveton_users");

            migrationBuilder.DropTable(
                name: "Owners");
        }
    }
}
