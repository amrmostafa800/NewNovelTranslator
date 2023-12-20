using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNovelIdFromEntityName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntityNames_Novels_NovelId",
                table: "EntityNames");

            migrationBuilder.DropIndex(
                name: "IX_EntityNames_NovelId",
                table: "EntityNames");

            migrationBuilder.DropColumn(
                name: "NovelId",
                table: "EntityNames");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "EntityNames",
                type: "VARCHAR(1)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "EntityNames",
                type: "VARCHAR",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(1)");

            migrationBuilder.AddColumn<int>(
                name: "NovelId",
                table: "EntityNames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EntityNames_NovelId",
                table: "EntityNames",
                column: "NovelId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntityNames_Novels_NovelId",
                table: "EntityNames",
                column: "NovelId",
                principalTable: "Novels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
