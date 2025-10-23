using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class AddPaperQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlankCount",
                table: "AppPaperQuestionRules");

            migrationBuilder.DropColumn(
                name: "BlankScore",
                table: "AppPaperQuestionRules");

            migrationBuilder.DropColumn(
                name: "JudgeCount",
                table: "AppPaperQuestionRules");

            migrationBuilder.DropColumn(
                name: "JudgeScore",
                table: "AppPaperQuestionRules");

            migrationBuilder.DropColumn(
                name: "MultiCount",
                table: "AppPaperQuestionRules");

            migrationBuilder.DropColumn(
                name: "MultiScore",
                table: "AppPaperQuestionRules");

            migrationBuilder.DropColumn(
                name: "Proportion",
                table: "AppPaperQuestionRules");

            migrationBuilder.DropColumn(
                name: "SingleCount",
                table: "AppPaperQuestionRules");

            migrationBuilder.DropColumn(
                name: "SingleScore",
                table: "AppPaperQuestionRules");

            migrationBuilder.RenameColumn(
                name: "PaperId",
                table: "AppPaperQuestionRules",
                newName: "PaperSectionId");

            migrationBuilder.AddColumn<int>(
                name: "PaperType",
                table: "AppPapers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "AppPaperQuestionRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionType",
                table: "AppPaperQuestionRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ScoreEach",
                table: "AppPaperQuestionRules",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "AppPaperSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PaperId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ScoreEach = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalScore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TotalCount = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPaperSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPaperSections_AppPapers_PaperId",
                        column: x => x.PaperId,
                        principalTable: "AppPapers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AppPaperQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PaperSectionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    QuestionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPaperQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPaperQuestions_AppPaperSections_PaperSectionId",
                        column: x => x.PaperSectionId,
                        principalTable: "AppPaperSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AppPaperQuestionRules_PaperSectionId",
                table: "AppPaperQuestionRules",
                column: "PaperSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPaperQuestions_PaperSectionId",
                table: "AppPaperQuestions",
                column: "PaperSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPaperSections_PaperId",
                table: "AppPaperSections",
                column: "PaperId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppPaperQuestionRules_AppPaperSections_PaperSectionId",
                table: "AppPaperQuestionRules",
                column: "PaperSectionId",
                principalTable: "AppPaperSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppPaperQuestionRules_AppPaperSections_PaperSectionId",
                table: "AppPaperQuestionRules");

            migrationBuilder.DropTable(
                name: "AppPaperQuestions");

            migrationBuilder.DropTable(
                name: "AppPaperSections");

            migrationBuilder.DropIndex(
                name: "IX_AppPaperQuestionRules_PaperSectionId",
                table: "AppPaperQuestionRules");

            migrationBuilder.DropColumn(
                name: "PaperType",
                table: "AppPapers");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "AppPaperQuestionRules");

            migrationBuilder.DropColumn(
                name: "QuestionType",
                table: "AppPaperQuestionRules");

            migrationBuilder.DropColumn(
                name: "ScoreEach",
                table: "AppPaperQuestionRules");

            migrationBuilder.RenameColumn(
                name: "PaperSectionId",
                table: "AppPaperQuestionRules",
                newName: "PaperId");

            migrationBuilder.AddColumn<int>(
                name: "BlankCount",
                table: "AppPaperQuestionRules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BlankScore",
                table: "AppPaperQuestionRules",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JudgeCount",
                table: "AppPaperQuestionRules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "JudgeScore",
                table: "AppPaperQuestionRules",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MultiCount",
                table: "AppPaperQuestionRules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MultiScore",
                table: "AppPaperQuestionRules",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Proportion",
                table: "AppPaperQuestionRules",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SingleCount",
                table: "AppPaperQuestionRules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SingleScore",
                table: "AppPaperQuestionRules",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }
    }
}