using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Purchase_Order_Request_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderLines_ProductionOrders_ProductionOrderID",
                table: "PurchaseOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_ProductionOrders_ProductionOrderID",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequestLines_ProductionOrders_ProductionOrdersId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_ProductionOrders_ProductionOrdersId",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_ProductionOrdersId",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequestLines_ProductionOrdersId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_ProductionOrderID",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderLines_ProductionOrderID",
                table: "PurchaseOrderLines");

            migrationBuilder.DropColumn(
                name: "ProductionOrdersId",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "ProductionOrdersId",
                table: "PurchaseRequestLines");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductionOrdersId",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductionOrdersId",
                table: "PurchaseRequestLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_ProductionOrdersId",
                table: "PurchaseRequests",
                column: "ProductionOrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_ProductionOrdersId",
                table: "PurchaseRequestLines",
                column: "ProductionOrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ProductionOrderID",
                table: "PurchaseOrders",
                column: "ProductionOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_ProductionOrderID",
                table: "PurchaseOrderLines",
                column: "ProductionOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLines_ProductionOrders_ProductionOrderID",
                table: "PurchaseOrderLines",
                column: "ProductionOrderID",
                principalTable: "ProductionOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_ProductionOrders_ProductionOrderID",
                table: "PurchaseOrders",
                column: "ProductionOrderID",
                principalTable: "ProductionOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequestLines_ProductionOrders_ProductionOrdersId",
                table: "PurchaseRequestLines",
                column: "ProductionOrdersId",
                principalTable: "ProductionOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_ProductionOrders_ProductionOrdersId",
                table: "PurchaseRequests",
                column: "ProductionOrdersId",
                principalTable: "ProductionOrders",
                principalColumn: "Id");
        }
    }
}
