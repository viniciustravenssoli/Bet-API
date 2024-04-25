using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bet.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Adding_Entity_Team_Fixing_Missing_Id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Teams_WinnerId",
                table: "Bets");

            migrationBuilder.AlterColumn<long>(
                name: "WinnerId",
                table: "Bets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Teams_WinnerId",
                table: "Bets",
                column: "WinnerId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Teams_WinnerId",
                table: "Bets");

            migrationBuilder.AlterColumn<long>(
                name: "WinnerId",
                table: "Bets",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Teams_WinnerId",
                table: "Bets",
                column: "WinnerId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
