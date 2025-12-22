using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class RenameToPlural : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserExamQuestion_AppUserExamSections_UserExamSectionId",
                table: "AppUserExamQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserExamQuestionReview_AppUserExamQuestion_UserExamQuesti~",
                table: "AppUserExamQuestionReview");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserExamSections_AppUserExam_UserExamId",
                table: "AppUserExamSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserExamQuestionReview",
                table: "AppUserExamQuestionReview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserExamQuestion",
                table: "AppUserExamQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserExam",
                table: "AppUserExam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppTraining",
                table: "AppTraining");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppExamination",
                table: "AppExamination");

            migrationBuilder.RenameTable(
                name: "AppUserExamQuestionReview",
                newName: "AppUserExamQuestionReviews");

            migrationBuilder.RenameTable(
                name: "AppUserExamQuestion",
                newName: "AppUserExamQuestions");

            migrationBuilder.RenameTable(
                name: "AppUserExam",
                newName: "AppUserExams");

            migrationBuilder.RenameTable(
                name: "AppTraining",
                newName: "AppTrainings");

            migrationBuilder.RenameTable(
                name: "AppExamination",
                newName: "AppExaminations");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserExamQuestionReview_UserExamQuestionId",
                table: "AppUserExamQuestionReviews",
                newName: "IX_AppUserExamQuestionReviews_UserExamQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserExamQuestion_UserExamSectionId",
                table: "AppUserExamQuestions",
                newName: "IX_AppUserExamQuestions_UserExamSectionId");

            migrationBuilder.RenameIndex(
                name: "IX_AppExamination_PaperId",
                table: "AppExaminations",
                newName: "IX_AppExaminations_PaperId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserExamQuestionReviews",
                table: "AppUserExamQuestionReviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserExamQuestions",
                table: "AppUserExamQuestions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserExams",
                table: "AppUserExams",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppTrainings",
                table: "AppTrainings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppExaminations",
                table: "AppExaminations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserExamQuestionReviews_AppUserExamQuestions_UserExamQues~",
                table: "AppUserExamQuestionReviews",
                column: "UserExamQuestionId",
                principalTable: "AppUserExamQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserExamQuestions_AppUserExamSections_UserExamSectionId",
                table: "AppUserExamQuestions",
                column: "UserExamSectionId",
                principalTable: "AppUserExamSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserExamSections_AppUserExams_UserExamId",
                table: "AppUserExamSections",
                column: "UserExamId",
                principalTable: "AppUserExams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserExamQuestionReviews_AppUserExamQuestions_UserExamQues~",
                table: "AppUserExamQuestionReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserExamQuestions_AppUserExamSections_UserExamSectionId",
                table: "AppUserExamQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserExamSections_AppUserExams_UserExamId",
                table: "AppUserExamSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserExams",
                table: "AppUserExams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserExamQuestions",
                table: "AppUserExamQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserExamQuestionReviews",
                table: "AppUserExamQuestionReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppTrainings",
                table: "AppTrainings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppExaminations",
                table: "AppExaminations");

            migrationBuilder.RenameTable(
                name: "AppUserExams",
                newName: "AppUserExam");

            migrationBuilder.RenameTable(
                name: "AppUserExamQuestions",
                newName: "AppUserExamQuestion");

            migrationBuilder.RenameTable(
                name: "AppUserExamQuestionReviews",
                newName: "AppUserExamQuestionReview");

            migrationBuilder.RenameTable(
                name: "AppTrainings",
                newName: "AppTraining");

            migrationBuilder.RenameTable(
                name: "AppExaminations",
                newName: "AppExamination");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserExamQuestions_UserExamSectionId",
                table: "AppUserExamQuestion",
                newName: "IX_AppUserExamQuestion_UserExamSectionId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserExamQuestionReviews_UserExamQuestionId",
                table: "AppUserExamQuestionReview",
                newName: "IX_AppUserExamQuestionReview_UserExamQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_AppExaminations_PaperId",
                table: "AppExamination",
                newName: "IX_AppExamination_PaperId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserExam",
                table: "AppUserExam",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserExamQuestion",
                table: "AppUserExamQuestion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserExamQuestionReview",
                table: "AppUserExamQuestionReview",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppTraining",
                table: "AppTraining",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppExamination",
                table: "AppExamination",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserExamQuestion_AppUserExamSections_UserExamSectionId",
                table: "AppUserExamQuestion",
                column: "UserExamSectionId",
                principalTable: "AppUserExamSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserExamQuestionReview_AppUserExamQuestion_UserExamQuesti~",
                table: "AppUserExamQuestionReview",
                column: "UserExamQuestionId",
                principalTable: "AppUserExamQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserExamSections_AppUserExam_UserExamId",
                table: "AppUserExamSections",
                column: "UserExamId",
                principalTable: "AppUserExam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}