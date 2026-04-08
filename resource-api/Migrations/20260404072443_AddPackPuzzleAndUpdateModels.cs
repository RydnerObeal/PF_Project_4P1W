using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace resource_api.Migrations
{
    /// <inheritdoc />
    public partial class AddPackPuzzleAndUpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Puzzles_Packs_PackId",
                table: "Puzzles");

            migrationBuilder.DropIndex(
                name: "IX_Puzzles_PackId",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "PackId",
                table: "Puzzles");

            migrationBuilder.AddColumn<string>(
                name: "Difficulty",
                table: "Puzzles",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hint",
                table: "Puzzles",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LibraryImageId",
                table: "Images",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PackPuzzles",
                columns: table => new
                {
                    PackId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PuzzleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackPuzzles", x => new { x.PackId, x.PuzzleId });
                    table.ForeignKey(
                        name: "FK_PackPuzzles_Packs_PackId",
                        column: x => x.PackId,
                        principalTable: "Packs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackPuzzles_Puzzles_PuzzleId",
                        column: x => x.PuzzleId,
                        principalTable: "Puzzles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_LibraryImageId",
                table: "Images",
                column: "LibraryImageId");

            migrationBuilder.CreateIndex(
                name: "IX_PackPuzzles_PuzzleId",
                table: "PackPuzzles",
                column: "PuzzleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_LibraryImages_LibraryImageId",
                table: "Images",
                column: "LibraryImageId",
                principalTable: "LibraryImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_LibraryImages_LibraryImageId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "PackPuzzles");

            migrationBuilder.DropIndex(
                name: "IX_Images_LibraryImageId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "Hint",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "LibraryImageId",
                table: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "PackId",
                table: "Puzzles",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Puzzles_PackId",
                table: "Puzzles",
                column: "PackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Puzzles_Packs_PackId",
                table: "Puzzles",
                column: "PackId",
                principalTable: "Packs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
