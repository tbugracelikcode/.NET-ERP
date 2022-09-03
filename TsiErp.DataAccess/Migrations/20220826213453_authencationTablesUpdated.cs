using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class authencationTablesUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Menus",
                table: "Menus");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "TsiUser");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "TsiUserRoles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "TsiRoles");

            migrationBuilder.RenameTable(
                name: "Menus",
                newName: "TsiMenus");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "TsiUser",
                type: "NVarChar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                table: "TsiUser",
                type: "NVarChar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "TsiUser",
                type: "NVarChar(95)",
                maxLength: 95,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "TsiUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TsiUser",
                type: "NVarChar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TsiUser",
                type: "Bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "EmailConfirmed",
                table: "TsiUser",
                type: "Bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "TsiUser",
                type: "NVarChar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "TsiUser",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "TsiUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TsiUserRoles",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "TsiUserRoles",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "TsiUserRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "TsiUserRoles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "TsiRoles",
                type: "NVarChar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "TsiRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "TsiRoles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentMenutId",
                table: "TsiMenus",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "MenuName",
                table: "TsiMenus",
                type: "NVarChar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TsiMenus",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TsiUser",
                table: "TsiUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TsiUserRoles",
                table: "TsiUserRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TsiRoles",
                table: "TsiRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TsiMenus",
                table: "TsiMenus",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TsiRolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    MenuId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TsiRolePermissions", x => new { x.MenuId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_TsiRolePermissions_TsiMenus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "TsiMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TsiRolePermissions_TsiRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "TsiRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TsiRolePermissions_RoleId",
                table: "TsiRolePermissions",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TsiRolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TsiUserRoles",
                table: "TsiUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TsiUser",
                table: "TsiUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TsiRoles",
                table: "TsiRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TsiMenus",
                table: "TsiMenus");

            migrationBuilder.RenameTable(
                name: "TsiUserRoles",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "TsiUser",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "TsiRoles",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "TsiMenus",
                newName: "Menus");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "UserRoles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(95)",
                oldMaxLength: 95);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "Bit");

            migrationBuilder.AlterColumn<bool>(
                name: "EmailConfirmed",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "Bit");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "Roles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentMenutId",
                table: "Menus",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<string>(
                name: "MenuName",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Menus",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Menus",
                table: "Menus",
                column: "Id");
        }
    }
}
