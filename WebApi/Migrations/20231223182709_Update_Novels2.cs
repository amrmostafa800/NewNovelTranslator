using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Update_Novels2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Novels_AspNetUsers_userId",
                table: "Novels");

            migrationBuilder.DropForeignKey(
                name: "FK_Novels_NovelNames_NovelNamesId",
                table: "Novels");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Novels",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "NovelNamesId",
                table: "Novels",
                newName: "NovelCloneId");

            migrationBuilder.RenameIndex(
                name: "IX_Novels_userId",
                table: "Novels",
                newName: "IX_Novels_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Novels_NovelNamesId",
                table: "Novels",
                newName: "IX_Novels_NovelCloneId");

            migrationBuilder.CreateTable(
                name: "NovelClones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NovelNameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NovelClones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NovelClones_NovelNames_NovelNameId",
                        column: x => x.NovelNameId,
                        principalTable: "NovelNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NovelClones_NovelNameId",
                table: "NovelClones",
                column: "NovelNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Novels_AspNetUsers_UserId",
                table: "Novels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Novels_NovelClones_NovelCloneId",
                table: "Novels",
                column: "NovelCloneId",
                principalTable: "NovelClones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Novels_AspNetUsers_UserId",
                table: "Novels");

            migrationBuilder.DropForeignKey(
                name: "FK_Novels_NovelClones_NovelCloneId",
                table: "Novels");

            migrationBuilder.DropTable(
                name: "NovelClones");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Novels",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "NovelCloneId",
                table: "Novels",
                newName: "NovelNamesId");

            migrationBuilder.RenameIndex(
                name: "IX_Novels_UserId",
                table: "Novels",
                newName: "IX_Novels_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Novels_NovelCloneId",
                table: "Novels",
                newName: "IX_Novels_NovelNamesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Novels_AspNetUsers_userId",
                table: "Novels",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Novels_NovelNames_NovelNamesId",
                table: "Novels",
                column: "NovelNamesId",
                principalTable: "NovelNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
