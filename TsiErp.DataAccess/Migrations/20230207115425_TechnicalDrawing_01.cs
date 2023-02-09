using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class TechnicalDrawing_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TechnicalDrawings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RevisionNo = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    RevisionDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Drawer = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    DrawingNo = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: true),
                    Description_ = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    DrawingDomain = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    DrawingFilePath = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    CustomerApproval = table.Column<bool>(type: "Bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "Bit", nullable: false),
                    SampleApproval = table.Column<bool>(type: "Bit", nullable: false),
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
                    table.PrimaryKey("PK_TechnicalDrawings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalDrawings_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalDrawings_ProductID",
                table: "TechnicalDrawings",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalDrawings_RevisionNo",
                table: "TechnicalDrawings",
                column: "RevisionNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TechnicalDrawings");
        }
    }
}
