using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBankingMindHub.Migrations
{
    /// <inheritdoc />
    public partial class AddCredits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientLoans_Loans_LoanId",
                table: "ClientLoans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Loans",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "CardColor",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardType",
                table: "Cards");

            migrationBuilder.RenameTable(
                name: "Loans",
                newName: "Credits");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Credits",
                table: "Credits",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLoans_Credits_LoanId",
                table: "ClientLoans",
                column: "LoanId",
                principalTable: "Credits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientLoans_Credits_LoanId",
                table: "ClientLoans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Credits",
                table: "Credits");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Cards");

            migrationBuilder.RenameTable(
                name: "Credits",
                newName: "Loans");

            migrationBuilder.AddColumn<int>(
                name: "CardColor",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CardType",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Loans",
                table: "Loans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLoans_Loans_LoanId",
                table: "ClientLoans",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
