using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Prod_Opr_Prod_Opr_Lines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductsOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    WorkCenterID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_ProductsOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsOperations_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductsOperationLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsOperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Priority = table.Column<int>(type: "Int", nullable: false),
                    ProcessQuantity = table.Column<int>(type: "Int", nullable: false),
                    AdjustmentAndControlTime = table.Column<int>(type: "Int", nullable: false),
                    OperationTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    Alternative = table.Column<bool>(type: "Bit", nullable: false),
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
                    table.PrimaryKey("PK_ProductsOperationLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsOperationLines_ProductsOperations_ProductsOperationID",
                        column: x => x.ProductsOperationID,
                        principalTable: "ProductsOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsOperationLines_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperationLines_ProductsOperationID",
                table: "ProductsOperationLines",
                column: "ProductsOperationID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperationLines_StationID",
                table: "ProductsOperationLines",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperations_Code",
                table: "ProductsOperations",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperations_ProductID",
                table: "ProductsOperations",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsOperationLines");

            migrationBuilder.DropTable(
                name: "ProductsOperations");
        }
    }
}
