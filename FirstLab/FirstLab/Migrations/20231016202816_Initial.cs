using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstLab.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlashcardSets",
                columns: table => new
                {
                    FlashcardSetName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardSets", x => x.FlashcardSetName);
                });

            migrationBuilder.CreateTable(
                name: "Flashcards",
                columns: table => new
                {
                    FlashcardName = table.Column<string>(type: "TEXT", nullable: false),
                    FlashcardQuestion = table.Column<string>(type: "TEXT", nullable: true),
                    FlashcardAnswer = table.Column<string>(type: "TEXT", nullable: true),
                    FlashcardColor = table.Column<string>(type: "TEXT", nullable: true),
                    FlashcardSetName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcards", x => x.FlashcardName);
                    table.ForeignKey(
                        name: "FK_Flashcards_FlashcardSets_FlashcardSetName",
                        column: x => x.FlashcardSetName,
                        principalTable: "FlashcardSets",
                        principalColumn: "FlashcardSetName");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_FlashcardSetName",
                table: "Flashcards",
                column: "FlashcardSetName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flashcards");

            migrationBuilder.DropTable(
                name: "FlashcardSets");
        }
    }
}
