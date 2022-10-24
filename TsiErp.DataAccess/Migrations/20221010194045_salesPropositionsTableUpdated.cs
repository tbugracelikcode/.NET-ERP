using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class salesPropositionsTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositionLines_SalesPropositions_SalesPropositionID",
                table: "SalesPropositionLines");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositionLines_SalesPropositions_SalesPropositionID",
                table: "SalesPropositionLines",
                column: "SalesPropositionID",
                principalTable: "SalesPropositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositionLines_SalesPropositions_SalesPropositionID",
                table: "SalesPropositionLines");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositionLines_SalesPropositions_SalesPropositionID",
                table: "SalesPropositionLines",
                column: "SalesPropositionID",
                principalTable: "SalesPropositions",
                principalColumn: "Id");
        }
    }
}
