using Microsoft.EntityFrameworkCore.Migrations;

namespace DressApp.WebUi.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HomeIsAppovred",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Kreaksiyon",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ozellik",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SalePrice",
                table: "Products",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "HomeIsAppovred",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Kreaksiyon",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Ozellik",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "Products");
        }
    }
}
