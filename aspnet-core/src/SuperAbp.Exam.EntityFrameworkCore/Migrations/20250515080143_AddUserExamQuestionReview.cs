using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class AddUserExamQuestionReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "AppUserExamQuestion",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppUserExamQuestionReview",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserExamQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserExamQuestionReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserExamQuestionReview_AppUserExamQuestion_UserExamQuestionId",
                        column: x => x.UserExamQuestionId,
                        principalTable: "AppUserExamQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestionAnswers_QuestionId",
                table: "AppQuestionAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserExamQuestionReview_UserExamQuestionId",
                table: "AppUserExamQuestionReview",
                column: "UserExamQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppQuestionAnswers_AppQuestions_QuestionId",
                table: "AppQuestionAnswers",
                column: "QuestionId",
                principalTable: "AppQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppQuestionAnswers_AppQuestions_QuestionId",
                table: "AppQuestionAnswers");

            migrationBuilder.DropTable(
                name: "AppUserExamQuestionReview");

            migrationBuilder.DropIndex(
                name: "IX_AppQuestionAnswers_QuestionId",
                table: "AppQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "AppUserExamQuestion");
        }
    }
}
