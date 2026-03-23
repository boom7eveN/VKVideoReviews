using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VKVideoReviews.DA.Migrations
{
    /// <inheritdoc />
    public partial class AppSessionsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTokens_UserId_IsActive",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserTokens");

            migrationBuilder.CreateTable(
                name: "UserAppSessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RefreshTokenHash = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAppSessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_UserAppSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAppSessions_RefreshTokenHash",
                table: "UserAppSessions",
                column: "RefreshTokenHash");

            migrationBuilder.CreateIndex(
                name: "IX_UserAppSessions_UserId",
                table: "UserAppSessions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAppSessions");

            migrationBuilder.DropIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserTokens",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId_IsActive",
                table: "UserTokens",
                columns: new[] { "UserId", "IsActive" });
        }
    }
}
