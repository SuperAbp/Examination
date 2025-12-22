using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class TableAddTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppUserExamQuestionReview",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppUserExamQuestion",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppUserExam",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppTraining",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppQuestions",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppQuestionKnowledgePoints",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppQuestionBanks",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppQuestionAnswers",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppPapers",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppPaperQuestionRules",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppMistakesReviews",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppKnowledgePoints",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppFavorites",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AppExamination",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppUserExamQuestionReview");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppUserExamQuestion");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppUserExam");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppTraining");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppQuestions");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppQuestionKnowledgePoints");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppQuestionBanks");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppPapers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppPaperQuestionRules");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppMistakesReviews");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppKnowledgePoints");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppFavorites");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppExamination");
        }
    }
}
