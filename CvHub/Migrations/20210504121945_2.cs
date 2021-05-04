using Microsoft.EntityFrameworkCore.Migrations;

namespace CvHub.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidationToken",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "GoogleId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "ValidationToken",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
