using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class salesPriceCurrentAccountCards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentAccountCardID",
                table: "SalesPrices",
                type: "UniqueIdentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPrices_CurrentAccountCards_CurrentAccountCardID",
                table: "SalesPrices",
                column: "CurrentAccountCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesPrices_CurrentAccountCards_CurrentAccountCardID",
                table: "SalesPrices");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentAccountCardID",
                table: "SalesPrices",
                type: "UniqueIdentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier",
                oldNullable: true);
        }
    }
}
