using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class roleTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TsiRolePermissions",
                table: "TsiRolePermissions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TsiRolePermissions",
                table: "TsiRolePermissions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TsiRolePermissions_MenuId",
                table: "TsiRolePermissions",
                column: "MenuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TsiRolePermissions",
                table: "TsiRolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_TsiRolePermissions_MenuId",
                table: "TsiRolePermissions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TsiRolePermissions",
                table: "TsiRolePermissions",
                columns: new[] { "MenuId", "RoleId" });
        }
    }
}
