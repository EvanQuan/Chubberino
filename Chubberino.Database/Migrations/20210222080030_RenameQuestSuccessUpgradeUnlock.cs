using Microsoft.EntityFrameworkCore.Migrations;

namespace Chubberino.Migrations
{
    public partial class RenameQuestSuccessUpgradeUnlock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextWorkerQuestSuccessUpgradeUnlock",
                table: "Players",
                newName: "NextQuestSuccessUpgradeUnlock");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextQuestSuccessUpgradeUnlock",
                table: "Players",
                newName: "NextWorkerQuestSuccessUpgradeUnlock");
        }
    }
}
