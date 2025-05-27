using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class UserExamQuestionAddForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppUserExamQuestion_UserExamId",
                table: "AppUserExamQuestion",
                column: "UserExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserExamQuestion_AppUserExam_UserExamId",
                table: "AppUserExamQuestion",
                column: "UserExamId",
                principalTable: "AppUserExam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserExamQuestion_AppUserExam_UserExamId",
                table: "AppUserExamQuestion");

            migrationBuilder.DropIndex(
                name: "IX_AppUserExamQuestion_UserExamId",
                table: "AppUserExamQuestion");
        }
    }
}
