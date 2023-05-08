using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashierDB.Migrations
{
    public partial class v15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceModifier",
                table: "Tab");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriceModifier",
                table: "Tab",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
