using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAbp73 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "SuperAbpMenu",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastPasswordChangeTime",
                table: "AbpUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShouldChangePasswordOnNextLogin",
                table: "AbpUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AbpUserDelegations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SourceUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserDelegations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpUserDelegations");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "SuperAbpMenu");

            migrationBuilder.DropColumn(
                name: "LastPasswordChangeTime",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "ShouldChangePasswordOnNextLogin",
                table: "AbpUsers");
        }
    }
}
