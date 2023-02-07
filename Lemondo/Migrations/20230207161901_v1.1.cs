using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lemondo.Migrations
{
    /// <inheritdoc />
    public partial class v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCheckedOut",
                table: "Book");

            migrationBuilder.AddColumn<int>(
                name: "BooksQuantity",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BooksQuantity",
                table: "Book");

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckedOut",
                table: "Book",
                type: "bit",
                nullable: true);
        }
    }
}
