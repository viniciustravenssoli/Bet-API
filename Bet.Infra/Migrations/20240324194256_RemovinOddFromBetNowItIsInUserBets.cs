using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bet.Infra.Migrations
{
    /// <inheritdoc />
    public partial class RemovinOddFromBetNowItIsInUserBets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Odd",
                table: "Bets");

            migrationBuilder.AddColumn<double>(
                name: "Odd",
                table: "UserBets",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Odd",
                table: "UserBets");

            migrationBuilder.AddColumn<double>(
                name: "Odd",
                table: "Bets",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
