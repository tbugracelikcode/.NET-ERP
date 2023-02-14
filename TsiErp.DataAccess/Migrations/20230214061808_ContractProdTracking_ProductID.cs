using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class ContractProdTracking_ProductID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductID",
                table: "ContractProductionTrackings",
                type: "UniqueIdentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractProductionTrackings_ProductID",
                table: "ContractProductionTrackings",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractProductionTrackings_Products_ProductID",
                table: "ContractProductionTrackings",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractProductionTrackings_Products_ProductID",
                table: "ContractProductionTrackings");

            migrationBuilder.DropIndex(
                name: "IX_ContractProductionTrackings_ProductID",
                table: "ContractProductionTrackings");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "ContractProductionTrackings");
        }
    }
}
