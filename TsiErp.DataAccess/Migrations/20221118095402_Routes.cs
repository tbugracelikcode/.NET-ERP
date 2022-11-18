using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Routes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteLines_Products_ProductsId",
                table: "RouteLines");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteLines_Routes_RouteID",
                table: "RouteLines");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteLines_TemplateOperations_TemplateOperationsId",
                table: "RouteLines");

            migrationBuilder.DropIndex(
                name: "IX_RouteLines_Code",
                table: "RouteLines");

            migrationBuilder.DropIndex(
                name: "IX_RouteLines_ProductsId",
                table: "RouteLines");

            migrationBuilder.DropIndex(
                name: "IX_RouteLines_TemplateOperationsId",
                table: "RouteLines");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "RouteLines");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RouteLines");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "RouteLines");

            migrationBuilder.DropColumn(
                name: "TemplateOperationsId",
                table: "RouteLines");

            migrationBuilder.RenameColumn(
                name: "OperationID",
                table: "RouteLines",
                newName: "ProductsOperationID");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_ProductID",
                table: "RouteLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_ProductsOperationID",
                table: "RouteLines",
                column: "ProductsOperationID");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLines_Products_ProductID",
                table: "RouteLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLines_ProductsOperations_ProductsOperationID",
                table: "RouteLines",
                column: "ProductsOperationID",
                principalTable: "ProductsOperations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLines_Routes_RouteID",
                table: "RouteLines",
                column: "RouteID",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteLines_Products_ProductID",
                table: "RouteLines");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteLines_ProductsOperations_ProductsOperationID",
                table: "RouteLines");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteLines_Routes_RouteID",
                table: "RouteLines");

            migrationBuilder.DropIndex(
                name: "IX_RouteLines_ProductID",
                table: "RouteLines");

            migrationBuilder.DropIndex(
                name: "IX_RouteLines_ProductsOperationID",
                table: "RouteLines");

            migrationBuilder.RenameColumn(
                name: "ProductsOperationID",
                table: "RouteLines",
                newName: "OperationID");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "RouteLines",
                type: "NVarChar(17)",
                maxLength: 17,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RouteLines",
                type: "NVarChar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                table: "RouteLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TemplateOperationsId",
                table: "RouteLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_Code",
                table: "RouteLines",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_ProductsId",
                table: "RouteLines",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLines_TemplateOperationsId",
                table: "RouteLines",
                column: "TemplateOperationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLines_Products_ProductsId",
                table: "RouteLines",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLines_Routes_RouteID",
                table: "RouteLines",
                column: "RouteID",
                principalTable: "Routes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLines_TemplateOperations_TemplateOperationsId",
                table: "RouteLines",
                column: "TemplateOperationsId",
                principalTable: "TemplateOperations",
                principalColumn: "Id");
        }
    }
}
