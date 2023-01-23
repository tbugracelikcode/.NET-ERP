using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Forecast_ForecastLine01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forecasts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    CreationDate_ = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ValidityStartDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    ValidityEndDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentAccountCardID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Total = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineNumber = table.Column<int>(type: "Int", nullable: false),
                    BranchID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PeriodID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_Forecasts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forecasts_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Forecasts_CurrentAccountCards_CurrentAccountCardID",
                        column: x => x.CurrentAccountCardID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Forecasts_Periods_PeriodID",
                        column: x => x.PeriodID,
                        principalTable: "Periods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ForecastLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ForecastID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    CustomerProductCode = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
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
                    table.PrimaryKey("PK_ForecastLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForecastLines_Forecasts_ForecastID",
                        column: x => x.ForecastID,
                        principalTable: "Forecasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForecastLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForecastLines_ForecastID",
                table: "ForecastLines",
                column: "ForecastID");

            migrationBuilder.CreateIndex(
                name: "IX_ForecastLines_ProductID",
                table: "ForecastLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Forecasts_BranchID",
                table: "Forecasts",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Forecasts_Code",
                table: "Forecasts",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Forecasts_CurrentAccountCardID",
                table: "Forecasts",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_Forecasts_PeriodID",
                table: "Forecasts",
                column: "PeriodID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForecastLines");

            migrationBuilder.DropTable(
                name: "Forecasts");
        }
    }
}
