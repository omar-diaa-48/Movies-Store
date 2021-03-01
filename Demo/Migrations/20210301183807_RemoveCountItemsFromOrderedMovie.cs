using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo.Migrations
{
    public partial class RemoveCountItemsFromOrderedMovie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountItems",
                table: "OrderedMovies");

            migrationBuilder.RenameColumn(
                name: "movieId",
                table: "OrderedMovies",
                newName: "MovieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "OrderedMovies",
                newName: "movieId");

            migrationBuilder.AddColumn<int>(
                name: "CountItems",
                table: "OrderedMovies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
