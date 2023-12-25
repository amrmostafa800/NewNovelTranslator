using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Update_Novels4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NovelUsers_NovelClones_NovelCloneId",
                table: "NovelUsers");

            migrationBuilder.DropTable(
                name: "NovelClones");

            migrationBuilder.CreateTable(
                name: "Novels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NovelNameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Novels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Novels_NovelNames_NovelNameId",
                        column: x => x.NovelNameId,
                        principalTable: "NovelNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Novels_NovelNameId",
                table: "Novels",
                column: "NovelNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_NovelUsers_Novels_NovelCloneId",
                table: "NovelUsers",
                column: "NovelCloneId",
                principalTable: "Novels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NovelUsers_Novels_NovelCloneId",
                table: "NovelUsers");

            migrationBuilder.DropTable(
                name: "Novels");

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
                name: "FK_NovelUsers_NovelClones_NovelCloneId",
                table: "NovelUsers",
                column: "NovelCloneId",
                principalTable: "NovelClones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
