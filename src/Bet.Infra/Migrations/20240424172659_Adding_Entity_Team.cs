using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bet.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Adding_Entity_Team : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChosenTeam",
                table: "UserBets",
                newName: "ChosenTeamId");

            migrationBuilder.RenameColumn(
                name: "Winner",
                table: "Bets",
                newName: "WinnerId");

            migrationBuilder.AddColumn<long>(
                name: "HomeId",
                table: "Bets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "VisitorId",
                table: "Bets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBets_ChosenTeamId",
                table: "UserBets",
                column: "ChosenTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_HomeId",
                table: "Bets",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_VisitorId",
                table: "Bets",
                column: "VisitorId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_WinnerId",
                table: "Bets",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Teams_HomeId",
                table: "Bets",
                column: "HomeId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Teams_VisitorId",
                table: "Bets",
                column: "VisitorId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Teams_WinnerId",
                table: "Bets",
                column: "WinnerId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBets_Teams_ChosenTeamId",
                table: "UserBets",
                column: "ChosenTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Teams_HomeId",
                table: "Bets");

            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Teams_VisitorId",
                table: "Bets");

            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Teams_WinnerId",
                table: "Bets");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBets_Teams_ChosenTeamId",
                table: "UserBets");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_UserBets_ChosenTeamId",
                table: "UserBets");

            migrationBuilder.DropIndex(
                name: "IX_Bets_HomeId",
                table: "Bets");

            migrationBuilder.DropIndex(
                name: "IX_Bets_VisitorId",
                table: "Bets");

            migrationBuilder.DropIndex(
                name: "IX_Bets_WinnerId",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "VisitorId",
                table: "Bets");

            migrationBuilder.RenameColumn(
                name: "ChosenTeamId",
                table: "UserBets",
                newName: "ChosenTeam");

            migrationBuilder.RenameColumn(
                name: "WinnerId",
                table: "Bets",
                newName: "Winner");
        }
    }
}
