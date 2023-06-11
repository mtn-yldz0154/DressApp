using Microsoft.EntityFrameworkCore.Migrations;

namespace DressApp.WebUi.Migrations
{
    public partial class Son3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikeItems_Likes_likeId",
                table: "LikeItems");

            migrationBuilder.RenameColumn(
                name: "likeId",
                table: "LikeItems",
                newName: "LikeId");

            migrationBuilder.RenameIndex(
                name: "IX_LikeItems_likeId",
                table: "LikeItems",
                newName: "IX_LikeItems_LikeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeItems_Likes_LikeId",
                table: "LikeItems",
                column: "LikeId",
                principalTable: "Likes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LikeItems_Likes_LikeId",
                table: "LikeItems");

            migrationBuilder.RenameColumn(
                name: "LikeId",
                table: "LikeItems",
                newName: "likeId");

            migrationBuilder.RenameIndex(
                name: "IX_LikeItems_LikeId",
                table: "LikeItems",
                newName: "IX_LikeItems_likeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeItems_Likes_likeId",
                table: "LikeItems",
                column: "likeId",
                principalTable: "Likes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
