using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class SalesPrices_WarehouseBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BranchID",
                table: "SalesPrices",
                type: "UniqueIdentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseID",
                table: "SalesPrices",
                type: "UniqueIdentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesPrices_BranchID",
                table: "SalesPrices",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPrices_WarehouseID",
                table: "SalesPrices",
                column: "WarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPrices_Branches_BranchID",
                table: "SalesPrices",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPrices_Warehouses_WarehouseID",
                table: "SalesPrices",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesPrices_Branches_BranchID",
                table: "SalesPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPrices_Warehouses_WarehouseID",
                table: "SalesPrices");

            migrationBuilder.DropIndex(
                name: "IX_SalesPrices_BranchID",
                table: "SalesPrices");

            migrationBuilder.DropIndex(
                name: "IX_SalesPrices_WarehouseID",
                table: "SalesPrices");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "SalesPrices");

            migrationBuilder.DropColumn(
                name: "WarehouseID",
                table: "SalesPrices");
        }
    }
}
