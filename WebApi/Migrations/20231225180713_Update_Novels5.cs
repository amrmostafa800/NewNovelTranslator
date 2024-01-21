using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Update_Novels5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NovelUsers_Novels_NovelCloneId",
                table: "NovelUsers");

            migrationBuilder.RenameColumn(
                name: "NovelCloneId",
                table: "NovelUsers",
                newName: "NovelId");

            migrationBuilder.RenameIndex(
                name: "IX_NovelUsers_NovelCloneId",
                table: "NovelUsers",
                newName: "IX_NovelUsers_NovelId");

            migrationBuilder.AddForeignKey(
                name: "FK_NovelUsers_Novels_NovelId",
                table: "NovelUsers",
                column: "NovelId",
                principalTable: "Novels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NovelUsers_Novels_NovelId",
                table: "NovelUsers");

            migrationBuilder.RenameColumn(
                name: "NovelId",
                table: "NovelUsers",
                newName: "NovelCloneId");

            migrationBuilder.RenameIndex(
                name: "IX_NovelUsers_NovelId",
                table: "NovelUsers",
                newName: "IX_NovelUsers_NovelCloneId");

            migrationBuilder.AddForeignKey(
                name: "FK_NovelUsers_Novels_NovelCloneId",
                table: "NovelUsers",
                column: "NovelCloneId",
                principalTable: "Novels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
