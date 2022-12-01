using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class authencationTablesDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TsiRolePermissions");

            migrationBuilder.DropTable(
                name: "TsiUser");

            migrationBuilder.DropTable(
                name: "TsiUserRoles");

            migrationBuilder.DropTable(
                name: "TsiMenus");

            migrationBuilder.DropTable(
                name: "TsiRoles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TsiMenus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    MenuName = table.Column<string>(type: "NVarChar(250)", maxLength: 250, nullable: false),
                    ParentMenutId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TsiMenus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TsiRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoleName = table.Column<string>(type: "NVarChar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TsiRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TsiUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "NVarChar(250)", maxLength: 250, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "Bit", nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "NVarChar(250)", maxLength: 250, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "NVarChar(95)", maxLength: 95, nullable: false),
                    Surname = table.Column<string>(type: "NVarChar(250)", maxLength: 250, nullable: false),
                    UserName = table.Column<string>(type: "NVarChar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TsiUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TsiUserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoleId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TsiUserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TsiRolePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TsiRolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TsiRolePermissions_TsiMenus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "TsiMenus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TsiRolePermissions_TsiRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "TsiRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TsiRolePermissions_MenuId",
                table: "TsiRolePermissions",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_TsiRolePermissions_RoleId",
                table: "TsiRolePermissions",
                column: "RoleId");
        }
    }
}
