using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VKVideoReviews.DA.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Videos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_CreatedAt",
                table: "Videos",
                column: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Videos_CreatedAt",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Videos");
        }
    }
}
