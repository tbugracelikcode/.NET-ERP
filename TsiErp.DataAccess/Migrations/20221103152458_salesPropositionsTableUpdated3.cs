using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class salesPropositionsTableUpdated3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositionLines_Branches_BranchesId",
                table: "SalesPropositionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositionLines_Warehouses_WarehousesId",
                table: "SalesPropositionLines");

            migrationBuilder.DropIndex(
                name: "IX_SalesPropositionLines_BranchesId",
                table: "SalesPropositionLines");

            migrationBuilder.DropIndex(
                name: "IX_SalesPropositionLines_WarehousesId",
                table: "SalesPropositionLines");

            migrationBuilder.DropColumn(
                name: "BranchesId",
                table: "SalesPropositionLines");

            migrationBuilder.DropColumn(
                name: "WarehousesId",
                table: "SalesPropositionLines");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BranchesId",
                table: "SalesPropositionLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehousesId",
                table: "SalesPropositionLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_BranchesId",
                table: "SalesPropositionLines",
                column: "BranchesId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_WarehousesId",
                table: "SalesPropositionLines",
                column: "WarehousesId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositionLines_Branches_BranchesId",
                table: "SalesPropositionLines",
                column: "BranchesId",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositionLines_Warehouses_WarehousesId",
                table: "SalesPropositionLines",
                column: "WarehousesId",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }
    }
}
