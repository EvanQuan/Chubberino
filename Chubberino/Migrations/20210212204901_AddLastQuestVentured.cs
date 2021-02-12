﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chubberino.Migrations
{
    public partial class AddLastQuestVentured : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastQuestVentured",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastQuestVentured",
                table: "Players");
        }
    }
}
