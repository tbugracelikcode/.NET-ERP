using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class ProductTrackings_Code : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "UnplannedMaintenances",
                type: "Int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Int");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ProductionTrackings",
                type: "NVarChar(17)",
                maxLength: 17,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "PlannedMaintenances",
                type: "Int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Int");

            migrationBuilder.AlterColumn<int>(
                name: "BloodType",
                table: "Employees",
                type: "Int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "Int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "ProductionTrackings");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "UnplannedMaintenances",
                type: "Int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "Int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "PlannedMaintenances",
                type: "Int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "Int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BloodType",
                table: "Employees",
                type: "Int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "Int",
                oldNullable: true);
        }
    }
}
