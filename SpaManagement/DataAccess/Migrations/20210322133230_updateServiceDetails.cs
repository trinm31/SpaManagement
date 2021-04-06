using Microsoft.EntityFrameworkCore.Migrations;

namespace SpaManagement.DataAccess.Migrations
{
    public partial class updateServiceDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "ServiceDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "ServiceDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
