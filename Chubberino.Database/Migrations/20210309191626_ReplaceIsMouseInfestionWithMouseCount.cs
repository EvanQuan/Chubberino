using Microsoft.EntityFrameworkCore.Migrations;

namespace Chubberino.Migrations
{
    public partial class ReplaceIsMouseInfestionWithMouseCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMouseInfested",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "MouseCount",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MouseCount",
                table: "Players");

            migrationBuilder.AddColumn<bool>(
                name: "IsMouseInfested",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
