using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Calendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillsofMaterialLines_Products_FinishedProductID",
                table: "BillsofMaterialLines");

            migrationBuilder.DropIndex(
                name: "IX_BillsofMaterialLines_FinishedProductID",
                table: "BillsofMaterialLines");

            migrationBuilder.DropColumn(
                name: "IsHalfDay",
                table: "CalendarDays");

            migrationBuilder.DropColumn(
                name: "IsHoliday",
                table: "CalendarDays");

            migrationBuilder.DropColumn(
                name: "IsMaintenance",
                table: "CalendarDays");

            migrationBuilder.DropColumn(
                name: "IsNotWorkDay",
                table: "CalendarDays");

            migrationBuilder.DropColumn(
                name: "IsOfficialHoliday",
                table: "CalendarDays");

            migrationBuilder.DropColumn(
                name: "IsShipmentDay",
                table: "CalendarDays");

            migrationBuilder.DropColumn(
                name: "IsWorkDay",
                table: "CalendarDays");

            migrationBuilder.AddColumn<int>(
                name: "CalendarDayStateEnum",
                table: "CalendarDays",
                type: "Int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                table: "CalendarDays",
                type: "NVarChar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_BillsofMaterialLines_Products_ProductID",
                table: "BillsofMaterialLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillsofMaterialLines_Products_ProductID",
                table: "BillsofMaterialLines");

            migrationBuilder.DropColumn(
                name: "CalendarDayStateEnum",
                table: "CalendarDays");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                table: "CalendarDays");

            migrationBuilder.AddColumn<bool>(
                name: "IsHalfDay",
                table: "CalendarDays",
                type: "Bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHoliday",
                table: "CalendarDays",
                type: "Bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMaintenance",
                table: "CalendarDays",
                type: "Bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNotWorkDay",
                table: "CalendarDays",
                type: "Bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOfficialHoliday",
                table: "CalendarDays",
                type: "Bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsShipmentDay",
                table: "CalendarDays",
                type: "Bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWorkDay",
                table: "CalendarDays",
                type: "Bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_BillsofMaterialLines_FinishedProductID",
                table: "BillsofMaterialLines",
                column: "FinishedProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_BillsofMaterialLines_Products_FinishedProductID",
                table: "BillsofMaterialLines",
                column: "FinishedProductID",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
