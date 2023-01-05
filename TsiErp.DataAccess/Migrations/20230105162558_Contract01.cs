using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Contract01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionTrackingHaltLines_ContractProductionTrackings_ContractProductionTrackingsId",
                table: "ProductionTrackingHaltLines");

            migrationBuilder.DropIndex(
                name: "IX_ProductionTrackingHaltLines_ContractProductionTrackingsId",
                table: "ProductionTrackingHaltLines");

            migrationBuilder.DropColumn(
                name: "ContractProductionTrackingsId",
                table: "ProductionTrackingHaltLines");

            migrationBuilder.DropColumn(
                name: "HaltTime",
                table: "ContractProductionTrackings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OperationEndDate",
                table: "ContractProductionTrackings",
                type: "DateTime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentAccountID",
                table: "ContractProductionTrackings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OperationEndTime",
                table: "ContractProductionTrackings",
                type: "time(7)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OperationStartTime",
                table: "ContractProductionTrackings",
                type: "time(7)",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "OperationEndTime",
                table: "ContractProductionTrackings");

            migrationBuilder.DropColumn(
                name: "OperationStartTime",
                table: "ContractProductionTrackings");

            migrationBuilder.AddColumn<Guid>(
                name: "ContractProductionTrackingsId",
                table: "ProductionTrackingHaltLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OperationEndDate",
                table: "ContractProductionTrackings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DateTime");

            migrationBuilder.AddColumn<decimal>(
                name: "HaltTime",
                table: "ContractProductionTrackings",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionTrackingHaltLines_ContractProductionTrackingsId",
                table: "ProductionTrackingHaltLines",
                column: "ContractProductionTrackingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionTrackingHaltLines_ContractProductionTrackings_ContractProductionTrackingsId",
                table: "ProductionTrackingHaltLines",
                column: "ContractProductionTrackingsId",
                principalTable: "ContractProductionTrackings",
                principalColumn: "Id");
        }
    }
}
