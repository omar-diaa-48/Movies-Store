using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo.Migrations
{
    public partial class AddOrdersListToMovie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Orders_OrderID1",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_OrderID1",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "OrderID1",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "MovieOrder",
                columns: table => new
                {
                    MovieID = table.Column<int>(type: "int", nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieOrder", x => new { x.MovieID, x.OrderID });
                    table.ForeignKey(
                        name: "FK_MovieOrder_Movies_MovieID",
                        column: x => x.MovieID,
                        principalTable: "Movies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieOrder_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieOrder_OrderID",
                table: "MovieOrder",
                column: "OrderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieOrder");

            migrationBuilder.AddColumn<int>(
                name: "OrderID1",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_OrderID1",
                table: "Movies",
                column: "OrderID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Orders_OrderID1",
                table: "Movies",
                column: "OrderID1",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
