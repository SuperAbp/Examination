using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class UserExamAddIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AppUserExams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(@"
                UPDATE AppUserExams
                SET IsActive = 1
                WHERE Id IN (
                    SELECT MAX(Id)
                    FROM AppUserExams
                    GROUP BY UserId, ExamId
                )
            ");

            migrationBuilder.Sql(@"
                UPDATE AppUserExams
                SET IsActive = 0
                WHERE IsActive IS NULL OR IsActive = 0
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AppUserExams");
        }
    }
}