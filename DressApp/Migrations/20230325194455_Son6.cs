using Microsoft.EntityFrameworkCore.Migrations;

namespace DressApp.WebUi.Migrations
{
    public partial class Son6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductUserItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    LongProductName = table.Column<string>(nullable: true),
                    ProductDescription = table.Column<string>(nullable: true),
                    ProductPrice = table.Column<double>(nullable: false),
                    ProductStock = table.Column<int>(nullable: false),
                    ProductSize = table.Column<string>(nullable: true),
                    ProductImageUrl = table.Column<string>(nullable: true),
                    SmallImageUrl = table.Column<string>(nullable: true),
                    FavoriAppovred = table.Column<bool>(nullable: false),
                    SepetAppovred = table.Column<bool>(nullable: false),
                    LikeApovred = table.Column<bool>(nullable: false),
                    StarNumber = table.Column<int>(nullable: false),
                    RayonId = table.Column<int>(nullable: false),
                    ProductUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUserItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductUserItems_ProductUsers_ProductUserId",
                        column: x => x.ProductUserId,
                        principalTable: "ProductUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductUserItems_Rayons_RayonId",
                        column: x => x.RayonId,
                        principalTable: "Rayons",
                        principalColumn: "RayonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductUserItems_ProductUserId",
                table: "ProductUserItems",
                column: "ProductUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUserItems_RayonId",
                table: "ProductUserItems",
                column: "RayonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductUserItems");

            migrationBuilder.DropTable(
                name: "ProductUsers");
        }
    }
}
