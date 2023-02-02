using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class PurchPrices_WarehouseBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BranchID",
                table: "PurchasePrices",
                type: "UniqueIdentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseID",
                table: "PurchasePrices",
                type: "UniqueIdentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePrices_BranchID",
                table: "PurchasePrices",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePrices_WarehouseID",
                table: "PurchasePrices",
                column: "WarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePrices_Branches_BranchID",
                table: "PurchasePrices",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePrices_Warehouses_WarehouseID",
                table: "PurchasePrices",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePrices_Branches_BranchID",
                table: "PurchasePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePrices_Warehouses_WarehouseID",
                table: "PurchasePrices");

            migrationBuilder.DropIndex(
                name: "IX_PurchasePrices_BranchID",
                table: "PurchasePrices");

            migrationBuilder.DropIndex(
                name: "IX_PurchasePrices_WarehouseID",
                table: "PurchasePrices");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "PurchasePrices");

            migrationBuilder.DropColumn(
                name: "WarehouseID",
                table: "PurchasePrices");
        }
    }
}
