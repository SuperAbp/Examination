using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class UpgradedToAbp10_7_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbpBackgroundJobs_IsAbandoned_NextTryTime",
                table: "AbpBackgroundJobs");

            migrationBuilder.AddColumn<bool>(
                name: "Leaved",
                table: "AbpUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyTypeFullName",
                table: "AbpEntityPropertyChanges",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "EntityTypeFullName",
                table: "AbpEntityChanges",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletionTime",
                table: "AbpBackgroundJobs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpBackgroundJobs_ApplicationName_CompletionTime_IsAbandone~",
                table: "AbpBackgroundJobs",
                columns: new[] { "ApplicationName", "CompletionTime", "IsAbandoned", "NextTryTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbpBackgroundJobs_ApplicationName_CompletionTime_IsAbandone~",
                table: "AbpBackgroundJobs");

            migrationBuilder.DropColumn(
                name: "Leaved",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "CompletionTime",
                table: "AbpBackgroundJobs");

            migrationBuilder.AlterColumn<string>(
                name: "PropertyTypeFullName",
                table: "AbpEntityPropertyChanges",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "EntityTypeFullName",
                table: "AbpEntityChanges",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.CreateIndex(
                name: "IX_AbpBackgroundJobs_IsAbandoned_NextTryTime",
                table: "AbpBackgroundJobs",
                columns: new[] { "IsAbandoned", "NextTryTime" });
        }
    }
}
