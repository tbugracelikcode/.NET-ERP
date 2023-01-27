using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class MaintenancePeriods_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaintenancePeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    PeriodTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
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
                    table.PrimaryKey("PK_MaintenancePeriods", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenancePeriods_Code",
                table: "MaintenancePeriods",
                column: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenancePeriods");
        }
    }
}
