using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class Update_Novel_Add_UserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Novels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "EntityNames",
                type: "VARCHAR(1)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR");

            migrationBuilder.CreateIndex(
                name: "IX_Novels_UserId",
                table: "Novels",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Novels_AspNetUsers_UserId",
                table: "Novels",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Novels_AspNetUsers_UserId",
                table: "Novels");

            migrationBuilder.DropIndex(
                name: "IX_Novels_UserId",
                table: "Novels");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Novels");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "EntityNames",
                type: "VARCHAR",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(1)");
        }
    }
}
