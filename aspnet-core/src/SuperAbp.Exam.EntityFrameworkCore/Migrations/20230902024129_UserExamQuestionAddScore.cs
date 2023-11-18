using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class UserExamQuestionAddScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "QuestionScore",
                table: "AppUserExamQuestion",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionScore",
                table: "AppUserExamQuestion");
        }
    }
}
