using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class UserExamAddAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "AppUserExamQuestion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "AppUserExamQuestion",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "AppUserExamQuestion",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "AppUserExamQuestion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppUserExamQuestion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "AppUserExamQuestion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "AppUserExamQuestion",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "AppUserExam",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "AppUserExam",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "AppUserExam",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppUserExam",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "AppUserExam",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "AppUserExam",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AppUserExamQuestion");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "AppUserExamQuestion");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "AppUserExamQuestion");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "AppUserExamQuestion");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppUserExamQuestion");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "AppUserExamQuestion");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "AppUserExamQuestion");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "AppUserExam");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "AppUserExam");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "AppUserExam");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppUserExam");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "AppUserExam");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "AppUserExam");
        }
    }
}
