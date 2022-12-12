using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Purchase_Order_Request_03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_PurchaseRequests_LinkedPurchaseRequestID",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_LinkedPurchaseRequestID",
                table: "PurchaseOrders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_LinkedPurchaseRequestID",
                table: "PurchaseOrders",
                column: "LinkedPurchaseRequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_PurchaseRequests_LinkedPurchaseRequestID",
                table: "PurchaseOrders",
                column: "LinkedPurchaseRequestID",
                principalTable: "PurchaseRequests",
                principalColumn: "Id");
        }
    }
}
