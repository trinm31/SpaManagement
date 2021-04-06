using Microsoft.EntityFrameworkCore.Migrations;

namespace SpaManagement.DataAccess.Migrations
{
    public partial class UpdateTableAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceDetailId",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ServiceDetailId",
                table: "Accounts");
        }
    }
}
