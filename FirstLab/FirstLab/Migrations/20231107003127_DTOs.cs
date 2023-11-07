using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstLab.Migrations
{
    /// <inheritdoc />
    public partial class DTOs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_FlashcardSets_FlashcardSetName",
                table: "Flashcards");

            migrationBuilder.RenameColumn(
                name: "FlashcardSetName",
                table: "Flashcards",
                newName: "FlashcardSetDTOFlashcardSetName");

            migrationBuilder.RenameIndex(
                name: "IX_Flashcards_FlashcardSetName",
                table: "Flashcards",
                newName: "IX_Flashcards_FlashcardSetDTOFlashcardSetName");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_FlashcardSets_FlashcardSetDTOFlashcardSetName",
                table: "Flashcards",
                column: "FlashcardSetDTOFlashcardSetName",
                principalTable: "FlashcardSets",
                principalColumn: "FlashcardSetName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_FlashcardSets_FlashcardSetDTOFlashcardSetName",
                table: "Flashcards");

            migrationBuilder.RenameColumn(
                name: "FlashcardSetDTOFlashcardSetName",
                table: "Flashcards",
                newName: "FlashcardSetName");

            migrationBuilder.RenameIndex(
                name: "IX_Flashcards_FlashcardSetDTOFlashcardSetName",
                table: "Flashcards",
                newName: "IX_Flashcards_FlashcardSetName");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_FlashcardSets_FlashcardSetName",
                table: "Flashcards",
                column: "FlashcardSetName",
                principalTable: "FlashcardSets",
                principalColumn: "FlashcardSetName");
        }
    }
}
