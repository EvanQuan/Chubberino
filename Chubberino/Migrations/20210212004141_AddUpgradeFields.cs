using Microsoft.EntityFrameworkCore.Migrations;

namespace Chubberino.Migrations
{
    public partial class AddUpgradeFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CheeseUnlocked",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaximumPointStorage",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PopulationCount",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkerCount",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheeseUnlocked",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "MaximumPointStorage",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PopulationCount",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "WorkerCount",
                table: "Players");
        }
    }
}
