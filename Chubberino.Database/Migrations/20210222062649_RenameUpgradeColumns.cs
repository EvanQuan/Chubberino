using Microsoft.EntityFrameworkCore.Migrations;

namespace Chubberino.Migrations
{
    public partial class RenameUpgradeColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastWorkerQuestHelpUnlocked",
                table: "Players",
                newName: "NextWorkerQuestSuccessUpgradeUnlock");

            migrationBuilder.RenameColumn(
                name: "LastWorkerProductionUpgradeUnlocked",
                table: "Players",
                newName: "NextWorkerProductionUpgradeUnlock");

            migrationBuilder.RenameColumn(
                name: "LastStorageUpgradeUnlocked",
                table: "Players",
                newName: "NextStorageUpgradeUnlock");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextWorkerQuestSuccessUpgradeUnlock",
                table: "Players",
                newName: "LastWorkerQuestHelpUnlocked");

            migrationBuilder.RenameColumn(
                name: "NextWorkerProductionUpgradeUnlock",
                table: "Players",
                newName: "LastWorkerProductionUpgradeUnlocked");

            migrationBuilder.RenameColumn(
                name: "NextStorageUpgradeUnlock",
                table: "Players",
                newName: "LastStorageUpgradeUnlocked");
        }
    }
}
