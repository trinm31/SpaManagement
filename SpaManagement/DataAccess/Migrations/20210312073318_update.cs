using Microsoft.EntityFrameworkCore.Migrations;

namespace SpaManagement.DataAccess.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "OrderDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "OrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
