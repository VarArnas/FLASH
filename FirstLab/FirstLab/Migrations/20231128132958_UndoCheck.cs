using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstLab.Migrations
{
    /// <inheritdoc />
    public partial class UndoCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Check",
                table: "Flashcards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Check",
                table: "Flashcards",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
