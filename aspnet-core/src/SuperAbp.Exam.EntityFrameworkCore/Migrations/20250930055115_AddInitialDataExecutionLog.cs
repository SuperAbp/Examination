using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperAbp.Exam.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialDataExecutionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS `InitialDataExecutionLog`;
                CREATE TABLE `InitialDataExecutionLog`  (
                  `Id` int NOT NULL AUTO_INCREMENT,
                  `LastExecutedTime` datetime NULL DEFAULT NULL,
                  PRIMARY KEY (`Id`) USING BTREE
                ) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS `InitialDataExecutionLog`");
        }
    }
}