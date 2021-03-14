using Microsoft.EntityFrameworkCore.Migrations;

namespace SpaManagement.DataAccess.Migrations
{
    public partial class BanlanceDebtCreditToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Accounts");

            migrationBuilder.AddColumn<double>(
                name: "BalanceCredit",
                table: "Accounts",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BalanceDebt",
                table: "Accounts",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceCredit",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "BalanceDebt",
                table: "Accounts");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Accounts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
