using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class AddUserExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserExam",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Finished = table.Column<bool>(type: "bit", nullable: false),
                    FinishedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserExam", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserExamQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Answers = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Right = table.Column<bool>(type: "bit", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserExamQuestion", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserExam");

            migrationBuilder.DropTable(
                name: "AppUserExamQuestion");
        }
    }
}
