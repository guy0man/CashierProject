using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashierDB.Migrations
{
    public partial class v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServedQuantity",
                table: "OrderList",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServedQuantity",
                table: "OrderList");
        }
    }
}
