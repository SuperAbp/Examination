using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class PaperRepoAddBlank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlankCount",
                table: "AppPaperRepositories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BlankScore",
                table: "AppPaperRepositories",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlankCount",
                table: "AppPaperRepositories");

            migrationBuilder.DropColumn(
                name: "BlankScore",
                table: "AppPaperRepositories");
        }
    }
}
