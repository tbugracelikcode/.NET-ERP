using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Shifts_Calendars_311022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    _Description = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    IsPlanned = table.Column<bool>(type: "Bit", nullable: false),
                    Year = table.Column<int>(type: "Int", nullable: false),
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
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "Bit", nullable: false),
                    TotalWorkTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    TotalBreakTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    NetWorkTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    Overtime = table.Column<decimal>(type: "Decimal", nullable: false),
                    ShiftOrder = table.Column<int>(type: "Int", nullable: false),
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
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftOverTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    ShiftTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    PlannedHaltTimes = table.Column<decimal>(type: "Decimal", nullable: false),
                    AvailableTime = table.Column<decimal>(type: "Decimal", nullable: false),
                    ShiftID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CalendarID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_CalendarLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarLines_Calendars_CalendarID",
                        column: x => x.CalendarID,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalendarLines_Shifts_ShiftID",
                        column: x => x.ShiftID,
                        principalTable: "Shifts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CalendarLines_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShiftLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LineNr = table.Column<int>(type: "int", nullable: false),
                    StartHour = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EndHour = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Type = table.Column<int>(type: "Int", nullable: false),
                    Coefficient = table.Column<decimal>(type: "Decimal", nullable: false),
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
                    table.PrimaryKey("PK_ShiftLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftLines_Shifts_ShiftID",
                        column: x => x.ShiftID,
                        principalTable: "Shifts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarLines_CalendarID",
                table: "CalendarLines",
                column: "CalendarID");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarLines_ShiftID",
                table: "CalendarLines",
                column: "ShiftID");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarLines_StationID",
                table: "CalendarLines",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_Code",
                table: "Calendars",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftLines_ShiftID",
                table: "ShiftLines",
                column: "ShiftID");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_Code",
                table: "Shifts",
                column: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarLines");

            migrationBuilder.DropTable(
                name: "ShiftLines");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "Shifts");
        }
    }
}
