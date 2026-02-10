using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class RenameAnnouncementTimeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublishTime",
                table: "AppAnnouncements",
                newName: "ScheduledPublishTime");

            migrationBuilder.RenameColumn(
                name: "ExpirationTime",
                table: "AppAnnouncements",
                newName: "ScheduledExpirationTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScheduledPublishTime",
                table: "AppAnnouncements",
                newName: "PublishTime");

            migrationBuilder.RenameColumn(
                name: "ScheduledExpirationTime",
                table: "AppAnnouncements",
                newName: "ExpirationTime");
        }
    }
}
