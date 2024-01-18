using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_Novels_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Novels_NovelNames_NovelNameId",
                table: "Novels");

            migrationBuilder.DropTable(
                name: "NovelNames");

            migrationBuilder.DropIndex(
                name: "IX_Novels_NovelNameId",
                table: "Novels");

            migrationBuilder.DropColumn(
                name: "NovelNameId",
                table: "Novels");

            migrationBuilder.AddColumn<string>(
                name: "NovelName",
                table: "Novels",
                type: "VARCHAR(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ArabicName",
                table: "EntityNames",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NovelName",
                table: "Novels");

            migrationBuilder.AddColumn<int>(
                name: "NovelNameId",
                table: "Novels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ArabicName",
                table: "EntityNames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
                name: "IX_Novels_NovelNameId",
                table: "Novels",
                column: "NovelNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Novels_NovelNames_NovelNameId",
                table: "Novels",
                column: "NovelNameId",
                principalTable: "NovelNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
