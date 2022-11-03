using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Shifts_DateTime2Span_311022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartHour",
                table: "ShiftLines",
                type: "time(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DateTime");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndHour",
                table: "ShiftLines",
                type: "time(7)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartHour",
                table: "ShiftLines",
                type: "DateTime",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(7)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndHour",
                table: "ShiftLines",
                type: "DateTime",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(7)");
        }
    }
}
