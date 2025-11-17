using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentActivities.Migrations
{
    /// <inheritdoc />
    public partial class AddDetailDescriptionColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Clubs_ClubId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Events_EventId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Resgistrations_Clubs_ClubId",
                table: "Resgistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Resgistrations_Events_EventId",
                table: "Resgistrations");

            migrationBuilder.DropIndex(
                name: "IX_Resgistrations_ClubId",
                table: "Resgistrations");

            migrationBuilder.DropIndex(
                name: "IX_Resgistrations_EventId",
                table: "Resgistrations");

            migrationBuilder.DropColumn(
                name: "DateResgistered",
                table: "Resgistrations");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Resgistrations",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Resgistrations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ClubId",
                table: "Resgistrations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "CancellationTime",
                table: "Resgistrations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckInTime",
                table: "Resgistrations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Resgistrations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PointsEarned",
                table: "Resgistrations",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "Resgistrations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Resgistrations",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Notifications",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ClubId",
                table: "Notifications",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationEndDate",
                table: "Events",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "TrainingPoints",
                table: "Events",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CurrentRegistrations",
                table: "Clubs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationEndDate",
                table: "Clubs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "TrainingPoints",
                table: "Clubs",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Checkins",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckInTime",
                table: "Checkins",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CheckedInBy",
                table: "Checkins",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resgistrations_ClubId",
                table: "Resgistrations",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Resgistrations_EventId",
                table: "Resgistrations",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkins_CheckedInBy",
                table: "Checkins",
                column: "CheckedInBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkins_Users_CheckedInBy",
                table: "Checkins",
                column: "CheckedInBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Clubs_ClubId",
                table: "Notifications",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Events_EventId",
                table: "Notifications",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Resgistrations_Clubs_ClubId",
                table: "Resgistrations",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Resgistrations_Events_EventId",
                table: "Resgistrations",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkins_Users_CheckedInBy",
                table: "Checkins");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Clubs_ClubId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Events_EventId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Resgistrations_Clubs_ClubId",
                table: "Resgistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Resgistrations_Events_EventId",
                table: "Resgistrations");

            migrationBuilder.DropIndex(
                name: "IX_Resgistrations_ClubId",
                table: "Resgistrations");

            migrationBuilder.DropIndex(
                name: "IX_Resgistrations_EventId",
                table: "Resgistrations");

            migrationBuilder.DropIndex(
                name: "IX_Checkins_CheckedInBy",
                table: "Checkins");

            migrationBuilder.DropColumn(
                name: "CancellationTime",
                table: "Resgistrations");

            migrationBuilder.DropColumn(
                name: "CheckInTime",
                table: "Resgistrations");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Resgistrations");

            migrationBuilder.DropColumn(
                name: "PointsEarned",
                table: "Resgistrations");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "Resgistrations");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Resgistrations");

            migrationBuilder.DropColumn(
                name: "RegistrationEndDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TrainingPoints",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CurrentRegistrations",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "RegistrationEndDate",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "TrainingPoints",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "CheckInTime",
                table: "Checkins");

            migrationBuilder.DropColumn(
                name: "CheckedInBy",
                table: "Checkins");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Resgistrations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Resgistrations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClubId",
                table: "Resgistrations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateResgistered",
                table: "Resgistrations",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClubId",
                table: "Notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Checkins",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Clubs_ClubId",
                table: "Notifications",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Events_EventId",
                table: "Notifications",
                column: "EventId",
                principalTable: "Events",
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
        }
    }
}
