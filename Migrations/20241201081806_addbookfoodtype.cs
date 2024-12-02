using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eproject.Migrations
{
    /// <inheritdoc />
    public partial class addbookfoodtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingFoodType",
                columns: table => new
                {
                    BookingFoodTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    FoodTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingFoodType", x => x.BookingFoodTypeId);
                    table.ForeignKey(
                        name: "FK_BookingFoodType_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingFoodType_foodtypes_FoodTypeId",
                        column: x => x.FoodTypeId,
                        principalTable: "foodtypes",
                        principalColumn: "FoodTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingFoodType_BookingId",
                table: "BookingFoodType",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingFoodType_FoodTypeId",
                table: "BookingFoodType",
                column: "FoodTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingFoodType");
        }
    }
}
