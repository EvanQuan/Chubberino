using Microsoft.EntityFrameworkCore.Migrations;

namespace Chubberino.Migrations
{
    public partial class RenameNextQuestUpgradeUnlock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextQuestRewardUpgradeUnlock",
                table: "Players",
                newName: "NextQuestUpgradeUnlock");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextQuestUpgradeUnlock",
                table: "Players",
                newName: "NextQuestRewardUpgradeUnlock");
        }
    }
}
