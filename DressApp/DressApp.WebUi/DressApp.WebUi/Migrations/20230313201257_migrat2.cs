using Microsoft.EntityFrameworkCore.Migrations;

namespace DressApp.WebUi.Migrations
{
    public partial class migrat2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StarNumber",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StarNumber",
                table: "Products");
        }
    }
}
