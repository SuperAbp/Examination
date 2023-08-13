using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class RenameExamingToExamination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppExamination",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PassingScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTime = table.Column<int>(type: "int", nullable: false),
                    PaperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_AppExamination", x => x.Id);
                });

            migrationBuilder.Sql("INSERT INTO AppExamination (Id,Name,Description,Score,PassingScore,TotalTime,PaperId,StartTime,EndTime,ExtraProperties,ConcurrencyStamp,CreationTime,CreatorId,LastModificationTime,LastModifierId,IsDeleted,DeleterId,DeletionTime) SELECT Id,Name,Description,Score,PassingScore,TotalTime,PaperId,StartTime,EndTime,ExtraProperties,ConcurrencyStamp,CreationTime,CreatorId,LastModificationTime,LastModifierId,IsDeleted,DeleterId,DeletionTime FROM AppExaming;");

            migrationBuilder.DropTable(
                name: "AppExaming");

            migrationBuilder.CreateIndex(
                name: "IX_AppExamination_PaperId",
                table: "AppExamination",
                column: "PaperId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppExaming",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PaperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassingScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppExaming", x => x.Id);
                });

            migrationBuilder.Sql("INSERT INTO AppExaming (Id,Name,Description,Score,PassingScore,TotalTime,PaperId,StartTime,EndTime,ExtraProperties,ConcurrencyStamp,CreationTime,CreatorId,LastModificationTime,LastModifierId,IsDeleted,DeleterId,DeletionTime) SELECT Id,Name,Description,Score,PassingScore,TotalTime,PaperId,StartTime,EndTime,ExtraProperties,ConcurrencyStamp,CreationTime,CreatorId,LastModificationTime,LastModifierId,IsDeleted,DeleterId,DeletionTime FROM AppExamination;");

            migrationBuilder.CreateIndex(
                name: "IX_AppExaming_PaperId",
                table: "AppExaming",
                column: "PaperId");

            migrationBuilder.DropTable(
                name: "AppExamination");
        }
    }
}