using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class PaperRepositoriesToPaperQuestionRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppPaperRepositories");

            migrationBuilder.CreateTable(
                name: "AppPaperQuestionRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionBankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Proportion = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SingleCount = table.Column<int>(type: "int", nullable: true),
                    SingleScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MultiCount = table.Column<int>(type: "int", nullable: true),
                    MultiScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    JudgeCount = table.Column<int>(type: "int", nullable: true),
                    JudgeScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BlankCount = table.Column<int>(type: "int", nullable: true),
                    BlankScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPaperQuestionRules", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppPaperQuestionRules");

            migrationBuilder.CreateTable(
                name: "AppPaperRepositories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlankCount = table.Column<int>(type: "int", nullable: true),
                    BlankScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    JudgeCount = table.Column<int>(type: "int", nullable: true),
                    JudgeScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MultiCount = table.Column<int>(type: "int", nullable: true),
                    MultiScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Proportion = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QuestionBankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SingleCount = table.Column<int>(type: "int", nullable: true),
                    SingleScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPaperRepositories", x => x.Id);
                });
        }
    }
}
