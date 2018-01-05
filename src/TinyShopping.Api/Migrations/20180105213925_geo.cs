using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TinyShopping.Api.Migrations
{
    public partial class geo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShoppingListId",
                table: "Items");

            migrationBuilder.AddColumn<double>(
                name: "Lat",
                table: "Stores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Lng",
                table: "Stores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<bool>(
                name: "Completed",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<DateTime>(
                name: "Done",
                table: "Items",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Lat",
                table: "Items",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ListId",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Lng",
                table: "Items",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Done",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ListId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "Items");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Completed",
                table: "Items",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingListId",
                table: "Items",
                nullable: false,
                defaultValue: 0);
        }
    }
}
