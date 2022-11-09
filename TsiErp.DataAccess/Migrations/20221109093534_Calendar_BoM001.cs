using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Calendar_BoM001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteLines_Products_ProductID",
                table: "RouteLines");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Products_ProductID",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ProductID",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_RouteLines_ProductID",
                table: "RouteLines");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                table: "Routes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                table: "RouteLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AvailableDays",
                table: "Calendars",
                type: "Decimal",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OfficialHolidayDays",
                table: "Calendars",
                type: "Decimal",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDays",
                table: "Calendars",
                type: "Decimal",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BillsofMaterials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    FinishedProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    RouteID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    _Description = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillsofMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillsofMaterials_Products_FinishedProductID",
                        column: x => x.FinishedProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CalendarDays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CalendarID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Date_ = table.Column<DateTime>(type: "Date", nullable: false),
                    IsWorkDay = table.Column<bool>(type: "Bit", nullable: false),
                    IsNotWorkDay = table.Column<bool>(type: "Bit", nullable: false),
                    IsOfficialHoliday = table.Column<bool>(type: "Bit", nullable: false),
                    IsHoliday = table.Column<bool>(type: "Bit", nullable: false),
                    IsHalfDay = table.Column<bool>(type: "Bit", nullable: false),
                    IsShipmentDay = table.Column<bool>(type: "Bit", nullable: false),
                    IsMaintenance = table.Column<bool>(type: "Bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarDays_Calendars_CalendarID",
                        column: x => x.CalendarID,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillsofMaterialLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoMID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    FinishedProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    RouteID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    MaterialType = table.Column<int>(type: "Int", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    QuantityFormula = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<decimal>(type: "Decimal", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    _Description = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    Size = table.Column<decimal>(type: "Decimal", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillsofMaterialLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillsofMaterialLines_BillsofMaterials_BoMID",
                        column: x => x.BoMID,
                        principalTable: "BillsofMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillsofMaterialLines_Products_FinishedProductID",
                        column: x => x.FinishedProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BillsofMaterialLines_UnitSets_UnitSetID",
                        column: x => x.UnitSetID,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ProductsId",
                table: "Routes",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_ProductsId",
                table: "RouteLines",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_BillsofMaterialLines_BoMID",
                table: "BillsofMaterialLines",
                column: "BoMID");

            migrationBuilder.CreateIndex(
                name: "IX_BillsofMaterialLines_FinishedProductID",
                table: "BillsofMaterialLines",
                column: "FinishedProductID");

            migrationBuilder.CreateIndex(
                name: "IX_BillsofMaterialLines_ProductID",
                table: "BillsofMaterialLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_BillsofMaterialLines_UnitSetID",
                table: "BillsofMaterialLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_BillsofMaterials_Code",
                table: "BillsofMaterials",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_BillsofMaterials_FinishedProductID",
                table: "BillsofMaterials",
                column: "FinishedProductID");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarDays_CalendarID",
                table: "CalendarDays",
                column: "CalendarID");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLines_Products_ProductsId",
                table: "RouteLines",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Products_ProductsId",
                table: "Routes",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteLines_Products_ProductsId",
                table: "RouteLines");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Products_ProductsId",
                table: "Routes");

            migrationBuilder.DropTable(
                name: "BillsofMaterialLines");

            migrationBuilder.DropTable(
                name: "CalendarDays");

            migrationBuilder.DropTable(
                name: "BillsofMaterials");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ProductsId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_RouteLines_ProductsId",
                table: "RouteLines");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "RouteLines");

            migrationBuilder.DropColumn(
                name: "AvailableDays",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "OfficialHolidayDays",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "TotalDays",
                table: "Calendars");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ProductID",
                table: "Routes",
                column: "ProductID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_ProductID",
                table: "RouteLines",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLines_Products_ProductID",
                table: "RouteLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Products_ProductID",
                table: "Routes",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
