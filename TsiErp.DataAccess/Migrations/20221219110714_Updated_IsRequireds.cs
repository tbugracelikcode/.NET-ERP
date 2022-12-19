using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Updated_IsRequireds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOperationLines_ProductsOperations_ProductsOperationsId",
                table: "ProductsOperationLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOperationLines_Stations_StationsId",
                table: "ProductsOperationLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOperations_Products_ProductsId",
                table: "ProductsOperations");

            migrationBuilder.DropIndex(
                name: "IX_ProductsOperations_ProductsId",
                table: "ProductsOperations");

            migrationBuilder.DropIndex(
                name: "IX_ProductsOperationLines_ProductsOperationsId",
                table: "ProductsOperationLines");

            migrationBuilder.DropIndex(
                name: "IX_ProductsOperationLines_StationsId",
                table: "ProductsOperationLines");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "ProductsOperations");

            migrationBuilder.DropColumn(
                name: "ProductsOperationsId",
                table: "ProductsOperationLines");

            migrationBuilder.DropColumn(
                name: "StationsId",
                table: "ProductsOperationLines");

            migrationBuilder.AlterColumn<string>(
                name: "ProductionStart",
                table: "Routes",
                type: "NVarChar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkCenterID",
                table: "ProductsOperations",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "TemplateOperationID",
                table: "ProductsOperations",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductID",
                table: "ProductsOperations",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductsOperations",
                type: "NVarChar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ProductsOperations",
                type: "Bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "ProductsOperations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "ProductsOperations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ProductsOperations",
                type: "NVarChar(17)",
                maxLength: 17,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StationID",
                table: "ProductsOperationLines",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductsOperationID",
                table: "ProductsOperationLines",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "ProcessQuantity",
                table: "ProductsOperationLines",
                type: "Int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "ProductsOperationLines",
                type: "Int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "OperationTime",
                table: "ProductsOperationLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<int>(
                name: "LineNr",
                table: "ProductsOperationLines",
                type: "Int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "ProductsOperationLines",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "ProductsOperationLines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Alternative",
                table: "ProductsOperationLines",
                type: "Bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<decimal>(
                name: "AdjustmentAndControlTime",
                table: "ProductsOperationLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperations_Code",
                table: "ProductsOperations",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperations_ProductID",
                table: "ProductsOperations",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperationLines_ProductsOperationID",
                table: "ProductsOperationLines",
                column: "ProductsOperationID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperationLines_StationID",
                table: "ProductsOperationLines",
                column: "StationID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOperationLines_ProductsOperations_ProductsOperationID",
                table: "ProductsOperationLines",
                column: "ProductsOperationID",
                principalTable: "ProductsOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOperationLines_Stations_StationID",
                table: "ProductsOperationLines",
                column: "StationID",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOperations_Products_ProductID",
                table: "ProductsOperations",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOperationLines_ProductsOperations_ProductsOperationID",
                table: "ProductsOperationLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOperationLines_Stations_StationID",
                table: "ProductsOperationLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOperations_Products_ProductID",
                table: "ProductsOperations");

            migrationBuilder.DropIndex(
                name: "IX_ProductsOperations_Code",
                table: "ProductsOperations");

            migrationBuilder.DropIndex(
                name: "IX_ProductsOperations_ProductID",
                table: "ProductsOperations");

            migrationBuilder.DropIndex(
                name: "IX_ProductsOperationLines_ProductsOperationID",
                table: "ProductsOperationLines");

            migrationBuilder.DropIndex(
                name: "IX_ProductsOperationLines_StationID",
                table: "ProductsOperationLines");

            migrationBuilder.AlterColumn<string>(
                name: "ProductionStart",
                table: "Routes",
                type: "NVarChar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVarChar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkCenterID",
                table: "ProductsOperations",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "TemplateOperationID",
                table: "ProductsOperations",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductID",
                table: "ProductsOperations",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductsOperations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ProductsOperations",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "Bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "ProductsOperations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "ProductsOperations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ProductsOperations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(17)",
                oldMaxLength: 17);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                table: "ProductsOperations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StationID",
                table: "ProductsOperationLines",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductsOperationID",
                table: "ProductsOperationLines",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<int>(
                name: "ProcessQuantity",
                table: "ProductsOperationLines",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Int");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "ProductsOperationLines",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Int");

            migrationBuilder.AlterColumn<decimal>(
                name: "OperationTime",
                table: "ProductsOperationLines",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<int>(
                name: "LineNr",
                table: "ProductsOperationLines",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Int");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "ProductsOperationLines",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "ProductsOperationLines",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "Alternative",
                table: "ProductsOperationLines",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "Bit");

            migrationBuilder.AlterColumn<decimal>(
                name: "AdjustmentAndControlTime",
                table: "ProductsOperationLines",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsOperationsId",
                table: "ProductsOperationLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StationsId",
                table: "ProductsOperationLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperations_ProductsId",
                table: "ProductsOperations",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperationLines_ProductsOperationsId",
                table: "ProductsOperationLines",
                column: "ProductsOperationsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperationLines_StationsId",
                table: "ProductsOperationLines",
                column: "StationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOperationLines_ProductsOperations_ProductsOperationsId",
                table: "ProductsOperationLines",
                column: "ProductsOperationsId",
                principalTable: "ProductsOperations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOperationLines_Stations_StationsId",
                table: "ProductsOperationLines",
                column: "StationsId",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOperations_Products_ProductsId",
                table: "ProductsOperations",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
