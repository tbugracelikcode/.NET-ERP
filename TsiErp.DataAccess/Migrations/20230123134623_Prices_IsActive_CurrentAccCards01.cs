using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Prices_IsActive_CurrentAccCards01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentAccountCardID",
                table: "SalesPrices",
                type: "UniqueIdentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SalesPrices",
                type: "Bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentAccountCardID",
                table: "SalesPriceLines",
                type: "UniqueIdentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentAccountCardID",
                table: "PurchasePrices",
                type: "UniqueIdentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PurchasePrices",
                type: "Bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentAccountCardID",
                table: "PurchasePriceLines",
                type: "UniqueIdentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SalesPrices_CurrentAccountCardID",
                table: "SalesPrices",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPriceLines_CurrentAccountCardID",
                table: "SalesPriceLines",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePrices_CurrentAccountCardID",
                table: "PurchasePrices",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePriceLines_CurrentAccountCardID",
                table: "PurchasePriceLines",
                column: "CurrentAccountCardID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SalesPrices_CurrentAccountCardID",
                table: "SalesPrices");

            migrationBuilder.DropIndex(
                name: "IX_SalesPriceLines_CurrentAccountCardID",
                table: "SalesPriceLines");

            migrationBuilder.DropIndex(
                name: "IX_PurchasePrices_CurrentAccountCardID",
                table: "PurchasePrices");

            migrationBuilder.DropIndex(
                name: "IX_PurchasePriceLines_CurrentAccountCardID",
                table: "PurchasePriceLines");

            migrationBuilder.DropColumn(
                name: "CurrentAccountCardID",
                table: "SalesPrices");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SalesPrices");

            migrationBuilder.DropColumn(
                name: "CurrentAccountCardID",
                table: "SalesPriceLines");

            migrationBuilder.DropColumn(
                name: "CurrentAccountCardID",
                table: "PurchasePrices");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PurchasePrices");

            migrationBuilder.DropColumn(
                name: "CurrentAccountCardID",
                table: "PurchasePriceLines");
        }
    }
}
