using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    public partial class AddExamingAndERepository : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppExamingRepositories",
                columns: table => new
                {
                    ExamingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionRepositoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RadioCount = table.Column<int>(type: "int", nullable: true),
                    RadioScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MultiCount = table.Column<int>(type: "int", nullable: true),
                    MultiScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    JudgeCount = table.Column<int>(type: "int", nullable: true),
                    JudgeScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppExamingRepositories", x => new { x.ExamingId, x.QuestionRepositoryId });
                });

            migrationBuilder.CreateTable(
                name: "AppExamings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PassingScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTime = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
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
                    table.PrimaryKey("PK_AppExamings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppExamingRepositories");

            migrationBuilder.DropTable(
                name: "AppExamings");
        }
    }
}
