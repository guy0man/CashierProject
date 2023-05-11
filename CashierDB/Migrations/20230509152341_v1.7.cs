using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashierDB.Migrations
{
    public partial class v17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceModifiersApplied_PriceModifier_PriceModifierLinkPriceModifierId",
                table: "PriceModifiersApplied");

            migrationBuilder.DropColumn(
                name: "PriceModifier",
                table: "PriceModifiersApplied");

            migrationBuilder.RenameColumn(
                name: "PriceModifierLinkPriceModifierId",
                table: "PriceModifiersApplied",
                newName: "PriceModifierId");

            migrationBuilder.RenameIndex(
                name: "IX_PriceModifiersApplied_PriceModifierLinkPriceModifierId",
                table: "PriceModifiersApplied",
                newName: "IX_PriceModifiersApplied_PriceModifierId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceModifiersApplied_PriceModifier_PriceModifierId",
                table: "PriceModifiersApplied",
                column: "PriceModifierId",
                principalTable: "PriceModifier",
                principalColumn: "PriceModifierId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceModifiersApplied_PriceModifier_PriceModifierId",
                table: "PriceModifiersApplied");

            migrationBuilder.RenameColumn(
                name: "PriceModifierId",
                table: "PriceModifiersApplied",
                newName: "PriceModifierLinkPriceModifierId");

            migrationBuilder.RenameIndex(
                name: "IX_PriceModifiersApplied_PriceModifierId",
                table: "PriceModifiersApplied",
                newName: "IX_PriceModifiersApplied_PriceModifierLinkPriceModifierId");

            migrationBuilder.AddColumn<int>(
                name: "PriceModifier",
                table: "PriceModifiersApplied",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceModifiersApplied_PriceModifier_PriceModifierLinkPriceModifierId",
                table: "PriceModifiersApplied",
                column: "PriceModifierLinkPriceModifierId",
                principalTable: "PriceModifier",
                principalColumn: "PriceModifierId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
