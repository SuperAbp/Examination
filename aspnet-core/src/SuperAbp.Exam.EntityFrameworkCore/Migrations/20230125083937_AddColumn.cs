using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    public partial class AddColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RadioScore",
                table: "AppExamingRepositories",
                newName: "SingleScore");

            migrationBuilder.RenameColumn(
                name: "RadioCount",
                table: "AppExamingRepositories",
                newName: "SingleCount");

            migrationBuilder.AddColumn<int>(
                name: "TotalQuestionCount",
                table: "AppExamings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Proportion",
                table: "AppExamingRepositories",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalQuestionCount",
                table: "AppExamings");

            migrationBuilder.DropColumn(
                name: "Proportion",
                table: "AppExamingRepositories");

            migrationBuilder.RenameColumn(
                name: "SingleScore",
                table: "AppExamingRepositories",
                newName: "RadioScore");

            migrationBuilder.RenameColumn(
                name: "SingleCount",
                table: "AppExamingRepositories",
                newName: "RadioCount");
        }
    }
}
