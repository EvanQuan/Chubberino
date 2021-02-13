﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Chubberino.Migrations
{
    public partial class AddStorageUpgradeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastStorageUpgradeUnlocked",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastStorageUpgradeUnlocked",
                table: "Players");
        }
    }
}
