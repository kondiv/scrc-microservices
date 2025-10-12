using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_token_user_UserId",
                table: "refresh_token");

            migrationBuilder.DropForeignKey(
                name: "FK_user_role_RoleId",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "user",
                newName: "role_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_RoleId",
                table: "user",
                newName: "IX_user_role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "refresh_token",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_refresh_token_UserId",
                table: "refresh_token",
                newName: "IX_refresh_token_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_token_user_user_id",
                table: "refresh_token",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_role_id",
                table: "user",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_token_user_user_id",
                table: "refresh_token");

            migrationBuilder.DropForeignKey(
                name: "FK_user_role_role_id",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "user",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_user_role_id",
                table: "user",
                newName: "IX_user_RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "refresh_token",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_refresh_token_user_id",
                table: "refresh_token",
                newName: "IX_refresh_token_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_token_user_UserId",
                table: "refresh_token",
                column: "UserId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_RoleId",
                table: "user",
                column: "RoleId",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
