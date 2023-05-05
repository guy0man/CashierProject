﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashierDB.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemType",
                columns: table => new
                {
                    ItemTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemType", x => x.ItemTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Tab",
                columns: table => new
                {
                    TabId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<float>(type: "real", nullable: false),
                    Tip = table.Column<float>(type: "real", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tab", x => x.TabId);
                });

            migrationBuilder.CreateTable(
                name: "MenuItem",
                columns: table => new
                {
                    MenuItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItem", x => x.MenuItemId);
                    table.ForeignKey(
                        name: "FK_MenuItem_ItemType_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemType",
                        principalColumn: "ItemTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderList",
                columns: table => new
                {
                    OrderListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    TabId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<float>(type: "real", nullable: false),
                    RealTotal = table.Column<float>(type: "real", nullable: false),
                    IsCanceled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderList", x => x.OrderListId);
                    table.ForeignKey(
                        name: "FK_OrderList_MenuItem_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItem",
                        principalColumn: "MenuItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderList_Tab_TabId",
                        column: x => x.TabId,
                        principalTable: "Tab",
                        principalColumn: "TabId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeId", "Name" },
                values: new object[] { 1, "Entree" });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeId", "Name" },
                values: new object[] { 2, "Drink" });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeId", "Name" },
                values: new object[] { 3, "Dessert" });

            migrationBuilder.InsertData(
                table: "MenuItem",
                columns: new[] { "MenuItemId", "ItemTypeId", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, 1, "Fried Chicken w/ Rice", 150f, 25 },
                    { 2, 1, "Ribs w/ Rice", 175f, 25 },
                    { 3, 1, "Sisig w/ Rice", 125f, 25 },
                    { 4, 2, "BTS Drink", 95f, 50 },
                    { 5, 2, "Sola", 99f, 50 },
                    { 6, 2, "Soft Drink in Can", 75f, 50 },
                    { 7, 3, "Cheesecake", 200f, 20 },
                    { 8, 3, "Moist Cake", 150f, 20 },
                    { 9, 3, "Sundae", 100f, 20 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_ItemTypeId",
                table: "MenuItem",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderList_MenuItemId",
                table: "OrderList",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderList_TabId",
                table: "OrderList",
                column: "TabId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderList");

            migrationBuilder.DropTable(
                name: "MenuItem");

            migrationBuilder.DropTable(
                name: "Tab");

            migrationBuilder.DropTable(
                name: "ItemType");
        }
    }
}
