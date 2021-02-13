using Microsoft.EntityFrameworkCore.Migrations;

namespace Chubberino.Migrations
{
    public partial class AddMouseTrapColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMouseInfested",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MouseTrapCount",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMouseInfested",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "MouseTrapCount",
                table: "Players");
        }
    }
}
