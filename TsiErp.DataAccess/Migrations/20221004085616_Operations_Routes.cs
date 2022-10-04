using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Operations_Routes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsContract",
                table: "Stations",
                type: "Bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFixtures",
                table: "Stations",
                type: "Bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    ProductionPoolID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_Operations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionStart = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Approval = table.Column<bool>(type: "Bit", nullable: false),
                    TechnicalApproval = table.Column<bool>(type: "Bit", nullable: false),
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
                    table.PrimaryKey("PK_Routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routes_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OperationLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    OperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "RouteLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    RouteID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    OperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionPoolID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionPoolDescription = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    AdjustmentAndControlTime = table.Column<int>(type: "Int", nullable: false),
                    OperationTime = table.Column<int>(type: "Int", nullable: false),
                    Priority = table.Column<int>(type: "Int", nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    OperationPicture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
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
                    table.PrimaryKey("PK_RouteLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteLines_Operations_OperationID",
                        column: x => x.OperationID,
                        principalTable: "Operations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteLines_Routes_RouteID",
                        column: x => x.RouteID,
                        principalTable: "Routes",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_Code",
                table: "RouteLines",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_OperationID",
                table: "RouteLines",
                column: "OperationID");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_ProductID",
                table: "RouteLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_RouteID",
                table: "RouteLines",
                column: "RouteID");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_Code",
                table: "Routes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ProductID",
                table: "Routes",
                column: "ProductID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationLines");

            migrationBuilder.DropTable(
                name: "RouteLines");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropColumn(
                name: "IsContract",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "IsFixtures",
                table: "Stations");
        }
    }
}
