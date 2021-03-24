using Microsoft.EntityFrameworkCore.Migrations;

namespace SpaManagement.DataAccess.Migrations
{
    public partial class FixServicedb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceUsers_ServiceDetails_ServiceDetailId",
                table: "ServiceUsers");

            migrationBuilder.DropIndex(
                name: "IX_ServiceUsers_ServiceDetailId",
                table: "ServiceUsers");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "ServiceUsers");

            migrationBuilder.DropColumn(
                name: "ServiceDetailId",
                table: "ServiceUsers");

            migrationBuilder.DropColumn(
                name: "Slottime",
                table: "ServiceDetails");

            migrationBuilder.DropColumn(
                name: "UseTime",
                table: "ServiceDetails");

            migrationBuilder.DropColumn(
                name: "UseTime",
                table: "CategoryServices");

            migrationBuilder.AddColumn<int>(
                name: "Slot",
                table: "ServiceDetails",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slot",
                table: "ServiceDetails");

            migrationBuilder.AddColumn<double>(
                name: "Paid",
                table: "ServiceUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceDetailId",
                table: "ServiceUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Slottime",
                table: "ServiceDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UseTime",
                table: "ServiceDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UseTime",
                table: "CategoryServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceUsers_ServiceDetailId",
                table: "ServiceUsers",
                column: "ServiceDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceUsers_ServiceDetails_ServiceDetailId",
                table: "ServiceUsers",
                column: "ServiceDetailId",
                principalTable: "ServiceDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
