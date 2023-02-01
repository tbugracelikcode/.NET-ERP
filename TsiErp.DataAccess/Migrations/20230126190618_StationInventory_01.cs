using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class StationInventory_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StationInventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_StationInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StationInventories_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StationInventories_ProductID",
                table: "StationInventories",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_StationInventories_StationID",
                table: "StationInventories",
                column: "StationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StationInventories");
        }
    }
}
