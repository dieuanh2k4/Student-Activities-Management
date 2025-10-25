using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentActivities.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDabate9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_Users_UsersId",
                table: "Clubs");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_UsersId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_UsersId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_UsersId",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Clubs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Clubs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_UsersId",
                table: "Events",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_UsersId",
                table: "Clubs",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_Users_UsersId",
                table: "Clubs",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_UsersId",
                table: "Events",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
