using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Update_Novels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Novels_NovelNames_NovelNamesId",
                table: "Novels");

            migrationBuilder.DropColumn(
                name: "novelNameId",
                table: "Novels");

            migrationBuilder.AlterColumn<int>(
                name: "NovelNamesId",
                table: "Novels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Novels_NovelNames_NovelNamesId",
                table: "Novels",
                column: "NovelNamesId",
                principalTable: "NovelNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Novels_NovelNames_NovelNamesId",
                table: "Novels");

            migrationBuilder.AlterColumn<int>(
                name: "NovelNamesId",
                table: "Novels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "novelNameId",
                table: "Novels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Novels_NovelNames_NovelNamesId",
                table: "Novels",
                column: "NovelNamesId",
                principalTable: "NovelNames",
                principalColumn: "Id");
        }
    }
}
