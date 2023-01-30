using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class String2TimeSpan_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Time_",
                table: "SalesPropositions",
                type: "time(7)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Time_",
                table: "SalesOrders",
                type: "time(7)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Time_",
                table: "PurchaseRequests",
                type: "time(7)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Time_",
                table: "PurchaseOrders",
                type: "time(7)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OperationEndDate",
                table: "ProductionTrackings",
                type: "DateTime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OperationEndTime",
                table: "ProductionTrackings",
                type: "time(7)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OperationStartTime",
                table: "ProductionTrackings",
                type: "time(7)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationEndTime",
                table: "ProductionTrackings");

            migrationBuilder.DropColumn(
                name: "OperationStartTime",
                table: "ProductionTrackings");

            migrationBuilder.AlterColumn<string>(
                name: "Time_",
                table: "SalesPropositions",
                type: "NVarChar(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Time_",
                table: "SalesOrders",
                type: "NVarChar(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Time_",
                table: "PurchaseRequests",
                type: "NVarChar(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Time_",
                table: "PurchaseOrders",
                type: "NVarChar(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OperationEndDate",
                table: "ProductionTrackings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DateTime");
        }
    }
}
