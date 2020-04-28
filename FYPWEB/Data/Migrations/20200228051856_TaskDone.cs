using Microsoft.EntityFrameworkCore.Migrations;

namespace FYPWEB.Data.Migrations
{
    public partial class TaskDone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaskDifficulty",
                table: "TaskDone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaskName",
                table: "TaskDone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskPoint",
                table: "TaskDone",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskDifficulty",
                table: "TaskDone");

            migrationBuilder.DropColumn(
                name: "TaskName",
                table: "TaskDone");

            migrationBuilder.DropColumn(
                name: "TaskPoint",
                table: "TaskDone");
        }
    }
}
