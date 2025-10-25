using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentActivities.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDabate8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "FK_Resgistrations_Users_StudentId",
                table: "Resgistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingScores_Users_StudentId",
                table: "TrainingScores");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AcademicClasses_AcademicClassId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_TrainingScores_Admins_TrainingScoresId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AcademicClassId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Admins_TrainingScoresId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Events_AdminsId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_AdminsId",
                table: "Clubs");

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
                name: "Birth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organizers_Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AdminsId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AdminsId",
                table: "Clubs");

            migrationBuilder.RenameColumn(
                name: "StudentsId",
                table: "Events",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_StudentsId",
                table: "Events",
                newName: "IX_Events_UsersId");

            migrationBuilder.RenameColumn(
                name: "StudentsId",
                table: "Clubs",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_Clubs_StudentsId",
                table: "Clubs",
                newName: "IX_Clubs_UsersId");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Birth = table.Column<DateOnly>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organizers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Birth = table.Column<DateOnly>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ClubId = table.Column<int>(type: "integer", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Birth = table.Column<DateOnly>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    AcademicClassId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_AcademicClasses_AcademicClassId",
                        column: x => x.AcademicClassId,
                        principalTable: "AcademicClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserId",
                table: "Admins",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizers_UserId",
                table: "Organizers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_AcademicClassId",
                table: "Students",
                column: "AcademicClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_Organizers_OrganizerId",
                table: "Clubs",
                column: "OrganizerId",
                principalTable: "Organizers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_Users_UsersId",
                table: "Clubs",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Organizers_OrganizerId",
                table: "Events",
                column: "OrganizerId",
                principalTable: "Organizers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_UsersId",
                table: "Events",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Admins_AdminId",
                table: "Reports",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Organizers_OrganizerId",
                table: "Reports",
                column: "OrganizerId",
                principalTable: "Organizers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resgistrations_Students_StudentId",
                table: "Resgistrations",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingScores_Students_StudentId",
                table: "TrainingScores",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_Organizers_OrganizerId",
                table: "Clubs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_Users_UsersId",
                table: "Clubs");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Organizers_OrganizerId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_UsersId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Admins_AdminId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Organizers_OrganizerId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Resgistrations_Students_StudentId",
                table: "Resgistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingScores_Students_StudentId",
                table: "TrainingScores");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Organizers");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "Events",
                newName: "StudentsId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_UsersId",
                table: "Events",
                newName: "IX_Events_StudentsId");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "Clubs",
                newName: "StudentsId");

            migrationBuilder.RenameIndex(
                name: "IX_Clubs_UsersId",
                table: "Clubs",
                newName: "IX_Clubs_StudentsId");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

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

            migrationBuilder.AddColumn<DateOnly>(
                name: "Birth",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "ClubId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Organizers_Role",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "Users",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AdminsId",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdminsId",
                table: "Clubs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AcademicClassId",
                table: "Users",
                column: "AcademicClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Admins_TrainingScoresId",
                table: "Users",
                column: "Admins_TrainingScoresId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_AdminsId",
                table: "Events",
                column: "AdminsId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_AdminsId",
                table: "Clubs",
                column: "AdminsId");

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
                name: "FK_Resgistrations_Users_StudentId",
                table: "Resgistrations",
                column: "StudentId",
                principalTable: "Users",
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
        }
    }
}
