using Microsoft.EntityFrameworkCore.Migrations;

namespace Chubberino.Migrations
{
    public partial class RenameQuestRewardUpgrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextQuestSuccessUpgradeUnlock",
                table: "Players",
                newName: "NextQuestRewardUpgradeUnlock");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextQuestRewardUpgradeUnlock",
                table: "Players",
                newName: "NextQuestSuccessUpgradeUnlock");
        }
    }
}
