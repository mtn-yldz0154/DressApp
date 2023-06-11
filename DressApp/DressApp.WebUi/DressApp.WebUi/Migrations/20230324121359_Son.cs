using Microsoft.EntityFrameworkCore.Migrations;

namespace DressApp.WebUi.Migrations
{
    public partial class Son : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LongProductName",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmallImageUrl",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LongProductName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SmallImageUrl",
                table: "Products");
        }
    }
}
