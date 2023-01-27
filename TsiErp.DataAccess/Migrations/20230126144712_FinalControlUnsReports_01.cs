using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class FinalControlUnsReports_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinalControlUnsuitabilityReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Date_ = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartyNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsScrap = table.Column<bool>(type: "Bit", nullable: false),
                    IsCorrection = table.Column<bool>(type: "Bit", nullable: false),
                    IsToBeUsedAs = table.Column<bool>(type: "Bit", nullable: false),
                    ControlFormDeclaration = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EmployeeID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_FinalControlUnsuitabilityReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinalControlUnsuitabilityReports_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinalControlUnsuitabilityReports_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinalControlUnsuitabilityReports_EmployeeID",
                table: "FinalControlUnsuitabilityReports",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_FinalControlUnsuitabilityReports_FicheNo",
                table: "FinalControlUnsuitabilityReports",
                column: "FicheNo");

            migrationBuilder.CreateIndex(
                name: "IX_FinalControlUnsuitabilityReports_ProductID",
                table: "FinalControlUnsuitabilityReports",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalControlUnsuitabilityReports");
        }
    }
}
