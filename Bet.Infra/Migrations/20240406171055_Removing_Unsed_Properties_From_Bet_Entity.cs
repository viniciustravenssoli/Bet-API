using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bet.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Removing_Unsed_Properties_From_Bet_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamA",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "TeamB",
                table: "Bets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamA",
                table: "Bets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamB",
                table: "Bets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
