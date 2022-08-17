using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tsi.Logging.EntityFrameworkCore.Migrations
{
    public partial class logTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Date_ = table.Column<DateTime>(type: "DateTime", maxLength: 200, nullable: false),
                    MethodName_ = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    BeforeValues = table.Column<object>(type: "sql_variant", maxLength: 5000, nullable: false),
                    AfterValues = table.Column<object>(type: "sql_variant", maxLength: 5000, nullable: false),
                    LogLevel_ = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
