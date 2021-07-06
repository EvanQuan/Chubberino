using Microsoft.EntityFrameworkCore.Migrations;

namespace Chubberino.Migrations
{
    public partial class AddUpgradeColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastWorkerProductionUpgradeUnlocked",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastWorkerQuestHelpUnlocked",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastWorkerProductionUpgradeUnlocked",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "LastWorkerQuestHelpUnlocked",
                table: "Players");
        }
    }
}
