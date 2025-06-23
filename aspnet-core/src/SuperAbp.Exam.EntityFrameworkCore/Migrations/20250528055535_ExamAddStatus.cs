using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class ExamAddStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "AppUserExam");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AppExamination",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppExamination");

            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "AppUserExam",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
