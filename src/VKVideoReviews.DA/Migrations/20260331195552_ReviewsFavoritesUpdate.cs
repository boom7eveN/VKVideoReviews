using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VKVideoReviews.DA.Migrations
{
    /// <inheritdoc />
    public partial class ReviewsFavoritesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Favorite_UserId",
                table: "Favorite");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId_VideoId",
                table: "Reviews",
                columns: new[] { "UserId", "VideoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_UserId_VideoId",
                table: "Favorite",
                columns: new[] { "UserId", "VideoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId_VideoId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Favorite_UserId_VideoId",
                table: "Favorite");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_UserId",
                table: "Favorite",
                column: "UserId");
        }
    }
}
