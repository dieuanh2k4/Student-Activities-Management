using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentActivities.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDabate10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_TrainingScores_TrainingScoresId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TrainingScoresId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TrainingScoresId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainingScoresId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TrainingScoresId",
                table: "Users",
                column: "TrainingScoresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_TrainingScores_TrainingScoresId",
                table: "Users",
                column: "TrainingScoresId",
                principalTable: "TrainingScores",
                principalColumn: "Id");
        }
    }
}
