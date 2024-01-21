using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Update_Novels_And_NovelNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NovelUsers");

            migrationBuilder.DropColumn(
                name: "novel_Name",
                table: "Novels");

            migrationBuilder.AddColumn<int>(
                name: "NovelNamesId",
                table: "Novels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "novelNameId",
                table: "Novels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "Novels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "NovelNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    novelName = table.Column<string>(type: "VARCHAR(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NovelNames", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Novels_NovelNamesId",
                table: "Novels",
                column: "NovelNamesId");

            migrationBuilder.CreateIndex(
                name: "IX_Novels_userId",
                table: "Novels",
                column: "userId");

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
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Novels_AspNetUsers_userId",
                table: "Novels");

            migrationBuilder.DropForeignKey(
                name: "FK_Novels_NovelNames_NovelNamesId",
                table: "Novels");

            migrationBuilder.DropTable(
                name: "NovelNames");

            migrationBuilder.DropIndex(
                name: "IX_Novels_NovelNamesId",
                table: "Novels");

            migrationBuilder.DropIndex(
                name: "IX_Novels_userId",
                table: "Novels");

            migrationBuilder.DropColumn(
                name: "NovelNamesId",
                table: "Novels");

            migrationBuilder.DropColumn(
                name: "novelNameId",
                table: "Novels");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Novels");

            migrationBuilder.AddColumn<string>(
                name: "novel_Name",
                table: "Novels",
                type: "VARCHAR(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "NovelUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    novelId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NovelUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NovelUsers_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NovelUsers_Novels_novelId",
                        column: x => x.novelId,
                        principalTable: "Novels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NovelUsers_novelId",
                table: "NovelUsers",
                column: "novelId");

            migrationBuilder.CreateIndex(
                name: "IX_NovelUsers_userId",
                table: "NovelUsers",
                column: "userId");
        }
    }
}
