using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class RouteLines02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductionPoolDescription",
                table: "RouteLines",
                type: "NVarChar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<decimal>(
                name: "OperationTime",
                table: "RouteLines",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductionPoolDescription",
                table: "RouteLines",
                type: "NVarChar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVarChar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OperationTime",
                table: "RouteLines",
                type: "Int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");
        }
    }
}
