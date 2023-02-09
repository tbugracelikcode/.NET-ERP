using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class ProductReferanceNo_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductReferanceNumbers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrentAccountCardID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ReferanceNo = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
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
                    table.PrimaryKey("PK_ProductReferanceNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductReferanceNumbers_CurrentAccountCards_CurrentAccountCardID",
                        column: x => x.CurrentAccountCardID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductReferanceNumbers_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductReferanceNumbers_CurrentAccountCardID",
                table: "ProductReferanceNumbers",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReferanceNumbers_ProductID",
                table: "ProductReferanceNumbers",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReferanceNumbers_ReferanceNo",
                table: "ProductReferanceNumbers",
                column: "ReferanceNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductReferanceNumbers");
        }
    }
}
