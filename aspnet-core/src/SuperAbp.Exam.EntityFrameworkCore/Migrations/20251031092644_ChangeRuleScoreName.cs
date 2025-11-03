using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRuleScoreName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserExamQuestion_AppUserExam_UserExamId",
                table: "AppUserExamQuestion");

            migrationBuilder.RenameColumn(
                name: "UserExamId",
                table: "AppUserExamQuestion",
                newName: "UserExamSectionId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserExamQuestion_UserExamId",
                table: "AppUserExamQuestion",
                newName: "IX_AppUserExamQuestion_UserExamSectionId");

            migrationBuilder.RenameColumn(
                name: "ScoreEach",
                table: "AppPaperQuestionRules",
                newName: "Score");

            migrationBuilder.CreateTable(
                name: "AppUserExamSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserExamId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SectionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ScoreEach = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalScore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TotalCount = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserExamSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserExamSections_AppUserExam_UserExamId",
                        column: x => x.UserExamId,
                        principalTable: "AppUserExam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserExamSections_UserExamId",
                table: "AppUserExamSections",
                column: "UserExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserExamQuestion_AppUserExamSections_UserExamSectionId",
                table: "AppUserExamQuestion",
                column: "UserExamSectionId",
                principalTable: "AppUserExamSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserExamQuestion_AppUserExamSections_UserExamSectionId",
                table: "AppUserExamQuestion");

            migrationBuilder.DropTable(
                name: "AppUserExamSections");

            migrationBuilder.RenameColumn(
                name: "UserExamSectionId",
                table: "AppUserExamQuestion",
                newName: "UserExamId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserExamQuestion_UserExamSectionId",
                table: "AppUserExamQuestion",
                newName: "IX_AppUserExamQuestion_UserExamId");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "AppPaperQuestionRules",
                newName: "ScoreEach");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserExamQuestion_AppUserExam_UserExamId",
                table: "AppUserExamQuestion",
                column: "UserExamId",
                principalTable: "AppUserExam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
