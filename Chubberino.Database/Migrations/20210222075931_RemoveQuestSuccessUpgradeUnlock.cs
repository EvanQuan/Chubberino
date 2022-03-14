using Microsoft.EntityFrameworkCore.Migrations;

namespace Chubberino.Migrations
{
    public partial class RemoveQuestSuccessUpgradeUnlock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextQuestSuccessUpgradeUnlock",
                table: "Players");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NextQuestSuccessUpgradeUnlock",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
