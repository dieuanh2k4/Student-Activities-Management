using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentActivities.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDabate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_Users_UserId",
                table: "Clubs");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_UserId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingScores_Users_UserId",
                table: "TrainingScores");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Semester",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TrainingScores",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainingScores_UserId",
                table: "TrainingScores",
                newName: "IX_TrainingScores_StudentId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Events",
                newName: "OrganizerId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_UserId",
                table: "Events",
                newName: "IX_Events_OrganizerId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Clubs",
                newName: "OrganizerId");

            migrationBuilder.RenameIndex(
                name: "IX_Clubs_UserId",
                table: "Clubs",
                newName: "IX_Clubs_OrganizerId");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<int>(
                name: "AcademicClassId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Admins_Role",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Admins_TrainingScoresId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Admins_UserId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClubId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organizers_Role",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Organizers_UserId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainingScoresId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SemesterId",
                table: "TrainingScores",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClubId",
                table: "Resgistrations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Resgistrations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Resgistrations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizerId",
                table: "Reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdminsId",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentsId",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdminsId",
                table: "Clubs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentsId",
                table: "Clubs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacultyId",
                table: "AcademicClasses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AcademicClassId",
                table: "Users",
                column: "AcademicClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Admins_TrainingScoresId",
                table: "Users",
                column: "Admins_TrainingScoresId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Admins_UserId",
                table: "Users",
                column: "Admins_UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Organizers_UserId",
                table: "Users",
                column: "Organizers_UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TrainingScoresId",
                table: "Users",
                column: "TrainingScoresId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingScores_SemesterId",
                table: "TrainingScores",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Resgistrations_ClubId",
                table: "Resgistrations",
                column: "ClubId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resgistrations_EventId",
                table: "Resgistrations",
                column: "EventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resgistrations_StudentId",
                table: "Resgistrations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AdminId",
                table: "Reports",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_OrganizerId",
                table: "Reports",
                column: "OrganizerId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_AdminsId",
                table: "Events",
                column: "AdminsId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StudentsId",
                table: "Events",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_AdminsId",
                table: "Clubs",
                column: "AdminsId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_StudentsId",
                table: "Clubs",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicClasses_FacultyId",
                table: "AcademicClasses",
                column: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicClasses_Faculties_FacultyId",
                table: "AcademicClasses",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_Users_AdminsId",
                table: "Clubs",
                column: "AdminsId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_Users_OrganizerId",
                table: "Clubs",
                column: "OrganizerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_Users_StudentsId",
                table: "Clubs",
                column: "StudentsId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_AdminsId",
                table: "Events",
                column: "AdminsId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_OrganizerId",
                table: "Events",
                column: "OrganizerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_StudentsId",
                table: "Events",
                column: "StudentsId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_AdminId",
                table: "Reports",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_OrganizerId",
                table: "Reports",
                column: "OrganizerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resgistrations_Clubs_ClubId",
                table: "Resgistrations",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resgistrations_Events_EventId",
                table: "Resgistrations",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resgistrations_Users_StudentId",
                table: "Resgistrations",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingScores_Semesters_SemesterId",
                table: "TrainingScores",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingScores_Users_StudentId",
                table: "TrainingScores",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AcademicClasses_AcademicClassId",
                table: "Users",
                column: "AcademicClassId",
                principalTable: "AcademicClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_TrainingScores_Admins_TrainingScoresId",
                table: "Users",
                column: "Admins_TrainingScoresId",
                principalTable: "TrainingScores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_TrainingScores_TrainingScoresId",
                table: "Users",
                column: "TrainingScoresId",
                principalTable: "TrainingScores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_Admins_UserId",
                table: "Users",
                column: "Admins_UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_Organizers_UserId",
                table: "Users",
                column: "Organizers_UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UserId",
                table: "Users",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicClasses_Faculties_FacultyId",
                table: "AcademicClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_Users_AdminsId",
                table: "Clubs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_Users_OrganizerId",
                table: "Clubs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_Users_StudentsId",
                table: "Clubs");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_AdminsId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_OrganizerId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_StudentsId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_AdminId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_OrganizerId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Resgistrations_Clubs_ClubId",
                table: "Resgistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Resgistrations_Events_EventId",
                table: "Resgistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Resgistrations_Users_StudentId",
                table: "Resgistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingScores_Semesters_SemesterId",
                table: "TrainingScores");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingScores_Users_StudentId",
                table: "TrainingScores");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AcademicClasses_AcademicClassId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_TrainingScores_Admins_TrainingScoresId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_TrainingScores_TrainingScoresId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_Admins_UserId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_Organizers_UserId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AcademicClassId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Admins_TrainingScoresId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Admins_UserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Organizers_UserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TrainingScoresId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TrainingScores_SemesterId",
                table: "TrainingScores");

            migrationBuilder.DropIndex(
                name: "IX_Resgistrations_ClubId",
                table: "Resgistrations");

            migrationBuilder.DropIndex(
                name: "IX_Resgistrations_EventId",
                table: "Resgistrations");

            migrationBuilder.DropIndex(
                name: "IX_Resgistrations_StudentId",
                table: "Resgistrations");

            migrationBuilder.DropIndex(
                name: "IX_Reports_AdminId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_OrganizerId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Events_AdminsId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_StudentsId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_AdminsId",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_StudentsId",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_AcademicClasses_FacultyId",
                table: "AcademicClasses");

            migrationBuilder.DropColumn(
                name: "AcademicClassId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Admins_Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Admins_TrainingScoresId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Admins_UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organizers_Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organizers_UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TrainingScoresId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "TrainingScores");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "Resgistrations");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Resgistrations");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Resgistrations");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "OrganizerId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AdminsId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "StudentsId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AdminsId",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "StudentsId",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "AcademicClasses");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "TrainingScores",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainingScores_StudentId",
                table: "TrainingScores",
                newName: "IX_TrainingScores_UserId");

            migrationBuilder.RenameColumn(
                name: "OrganizerId",
                table: "Events",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_OrganizerId",
                table: "Events",
                newName: "IX_Events_UserId");

            migrationBuilder.RenameColumn(
                name: "OrganizerId",
                table: "Clubs",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Clubs_OrganizerId",
                table: "Clubs",
                newName: "IX_Clubs_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "integer",
                maxLength: 10,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "Users",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Faculty",
                table: "Users",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Semester",
                table: "Users",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_Users_UserId",
                table: "Clubs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_UserId",
                table: "Events",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingScores_Users_UserId",
                table: "TrainingScores",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
