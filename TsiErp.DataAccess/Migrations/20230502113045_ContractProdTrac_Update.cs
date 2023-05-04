using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class ContractProdTrac_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractProductionTrackings_CurrentAccountCards_ShiftID",
                table: "ContractProductionTrackings");

            migrationBuilder.DropIndex(
                name: "IX_ContractProductionTrackings_CurrentAccountID",
                table: "ContractProductionTrackings");

            migrationBuilder.DropColumn(
                name: "CurrentAccountID",
                table: "ContractProductionTrackings");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentAccountCardID",
                table: "ContractProductionTrackings",
                type: "UniqueIdentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ContractProductionTrackings_CurrentAccountCardID",
                table: "ContractProductionTrackings",
                column: "CurrentAccountCardID");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractProductionTrackings_CurrentAccountCards_CurrentAccountCardID",
                table: "ContractProductionTrackings",
                column: "CurrentAccountCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractProductionTrackings_CurrentAccountCards_CurrentAccountCardID",
                table: "ContractProductionTrackings");

            migrationBuilder.DropIndex(
                name: "IX_ContractProductionTrackings_CurrentAccountCardID",
                table: "ContractProductionTrackings");

            migrationBuilder.DropColumn(
                name: "CurrentAccountCardID",
                table: "ContractProductionTrackings");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentAccountID",
                table: "ContractProductionTrackings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ContractProductionTrackings_CurrentAccountID",
                table: "ContractProductionTrackings",
                column: "CurrentAccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractProductionTrackings_CurrentAccountCards_ShiftID",
                table: "ContractProductionTrackings",
                column: "ShiftID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");
        }
    }
}
