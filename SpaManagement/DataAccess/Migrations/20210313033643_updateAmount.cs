using Microsoft.EntityFrameworkCore.Migrations;

namespace SpaManagement.DataAccess.Migrations
{
    public partial class updateAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Accounts");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Accounts",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Accounts");

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "Accounts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
