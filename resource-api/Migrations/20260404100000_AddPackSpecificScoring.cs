using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace resource_api.Migrations
{
    /// <inheritdoc />
    public partial class AddPackSpecificScoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaseScore",
                table: "Packs",
                type: "int",
                nullable: false,
                defaultValue: 10);

            migrationBuilder.AddColumn<Guid>(
                name: "PackId",
                table: "GameScores",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_GameScores_PackId",
                table: "GameScores",
                column: "PackId");

            // SQLite doesn't support adding foreign keys to existing tables
            // migrationBuilder.AddForeignKey(
            //     name: "FK_GameScores_Packs_PackId",
            //     table: "GameScores",
            //     column: "PackId",
            //     principalTable: "Packs",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // SQLite doesn't support dropping foreign keys from existing tables
            // migrationBuilder.DropForeignKey(
            //     name: "FK_GameScores_Packs_PackId",
            //     table: "GameScores");

            migrationBuilder.DropIndex(
                name: "IX_GameScores_PackId",
                table: "GameScores");

            migrationBuilder.DropColumn(
                name: "BaseScore",
                table: "Packs");

            migrationBuilder.DropColumn(
                name: "PackId",
                table: "GameScores");
        }
    }
}
