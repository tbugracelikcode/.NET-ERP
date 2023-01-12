using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class SchedulerLine_date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date_",
                table: "CalendarLines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date_",
                table: "CalendarLines");
        }
    }
}
