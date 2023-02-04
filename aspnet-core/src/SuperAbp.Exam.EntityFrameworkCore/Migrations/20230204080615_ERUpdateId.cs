using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    public partial class ERUpdateId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppExamingRepositories",
                table: "AppExamingRepositories");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AppExamingRepositories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppExamingRepositories",
                table: "AppExamingRepositories",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppExamingRepositories",
                table: "AppExamingRepositories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AppExamingRepositories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppExamingRepositories",
                table: "AppExamingRepositories",
                columns: new[] { "ExamingId", "QuestionRepositoryId" });
        }
    }
}
