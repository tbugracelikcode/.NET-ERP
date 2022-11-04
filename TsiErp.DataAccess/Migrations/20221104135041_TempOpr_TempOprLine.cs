using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class TempOpr_TempOprLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteLines_Operations_OperationID",
                table: "RouteLines");

            migrationBuilder.DropTable(
                name: "OperationLines");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_RouteLines_OperationID",
                table: "RouteLines");

            migrationBuilder.AddColumn<Guid>(
                name: "TemplateOperationsId",
                table: "RouteLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TemplateOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    WorkCenterID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_TemplateOperations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateOperationLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateOperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Priority = table.Column<int>(type: "Int", nullable: false),
                    ProcessQuantity = table.Column<int>(type: "Int", nullable: false),
                    AdjustmentAndControlTime = table.Column<int>(type: "Int", nullable: false),
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
                    table.PrimaryKey("PK_TemplateOperationLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateOperationLines_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TemplateOperationLines_TemplateOperations_TemplateOperationID",
                        column: x => x.TemplateOperationID,
                        principalTable: "TemplateOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_TemplateOperationsId",
                table: "RouteLines",
                column: "TemplateOperationsId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateOperationLines_StationID",
                table: "TemplateOperationLines",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateOperationLines_TemplateOperationID",
                table: "TemplateOperationLines",
                column: "TemplateOperationID");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateOperations_Code",
                table: "TemplateOperations",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLines_TemplateOperations_TemplateOperationsId",
                table: "RouteLines",
                column: "TemplateOperationsId",
                principalTable: "TemplateOperations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteLines_TemplateOperations_TemplateOperationsId",
                table: "RouteLines");

            migrationBuilder.DropTable(
                name: "TemplateOperationLines");

            migrationBuilder.DropTable(
                name: "TemplateOperations");

            migrationBuilder.DropIndex(
                name: "IX_RouteLines_TemplateOperationsId",
                table: "RouteLines");

            migrationBuilder.DropColumn(
                name: "TemplateOperationsId",
                table: "RouteLines");

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    ProductionPoolID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    AdjustmentAndControlTime = table.Column<int>(type: "Int", nullable: false),
                    Alternative = table.Column<bool>(type: "Bit", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Priority = table.Column<int>(type: "Int", nullable: false),
                    ProcessQuantity = table.Column<int>(type: "Int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationLines_Operations_OperationID",
                        column: x => x.OperationID,
                        principalTable: "Operations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationLines_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_OperationID",
                table: "RouteLines",
                column: "OperationID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationLines_Code",
                table: "OperationLines",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_OperationLines_OperationID",
                table: "OperationLines",
                column: "OperationID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationLines_StationID",
                table: "OperationLines",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_Code",
                table: "Operations",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLines_Operations_OperationID",
                table: "RouteLines",
                column: "OperationID",
                principalTable: "Operations",
                principalColumn: "Id");
        }
    }
}
