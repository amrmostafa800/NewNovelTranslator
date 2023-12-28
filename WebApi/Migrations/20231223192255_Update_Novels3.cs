using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
	/// <inheritdoc />
	public partial class Update_Novels3 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Novels");

			migrationBuilder.CreateTable(
				name: "NovelUsers",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					NovelCloneId = table.Column<int>(type: "int", nullable: false),
					UserId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_NovelUsers", x => x.Id);
					table.ForeignKey(
						name: "FK_NovelUsers_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_NovelUsers_NovelClones_NovelCloneId",
						column: x => x.NovelCloneId,
						principalTable: "NovelClones",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_NovelUsers_NovelCloneId",
				table: "NovelUsers",
				column: "NovelCloneId");

			migrationBuilder.CreateIndex(
				name: "IX_NovelUsers_UserId",
				table: "NovelUsers",
				column: "UserId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "NovelUsers");

			migrationBuilder.CreateTable(
				name: "Novels",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					NovelCloneId = table.Column<int>(type: "int", nullable: false),
					UserId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Novels", x => x.Id);
					table.ForeignKey(
						name: "FK_Novels_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Novels_NovelClones_NovelCloneId",
						column: x => x.NovelCloneId,
						principalTable: "NovelClones",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Novels_NovelCloneId",
				table: "Novels",
				column: "NovelCloneId");

			migrationBuilder.CreateIndex(
				name: "IX_Novels_UserId",
				table: "Novels",
				column: "UserId");
		}
	}
}
