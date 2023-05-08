using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashierDB.Migrations
{
    public partial class v13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceModifier_Company_CompanyId",
                table: "PriceModifier");

            migrationBuilder.DropIndex(
                name: "IX_PriceModifier_CompanyId",
                table: "PriceModifier");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "PriceModifier");

            migrationBuilder.AddColumn<int>(
                name: "Percentage",
                table: "PriceModifier",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "PriceModifier");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "PriceModifier",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceModifier_CompanyId",
                table: "PriceModifier",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceModifier_Company_CompanyId",
                table: "PriceModifier",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId");
        }
    }
}
