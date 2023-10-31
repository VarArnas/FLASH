using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstLab.Migrations
{
    /// <inheritdoc />
    public partial class FlashcardTimer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FlashcardTimer",
                table: "Flashcards",
                type: "TEXT",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlashcardTimer",
                table: "Flashcards");
        }
    }
}
