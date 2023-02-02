using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class satinalmaFiyatTablosuCariHesap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentAccountCardID",
                table: "PurchasePrices",
                type: "UniqueIdentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePrices_CurrentAccountCards_CurrentAccountCardID",
                table: "PurchasePrices",
                column: "CurrentAccountCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePrices_CurrentAccountCards_CurrentAccountCardID",
                table: "PurchasePrices");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentAccountCardID",
                table: "PurchasePrices",
                type: "UniqueIdentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier",
                oldNullable: true);
        }
    }
}
