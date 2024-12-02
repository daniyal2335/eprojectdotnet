using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eproject.Migrations
{
    /// <inheritdoc />
    public partial class status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_foodtypes_Bookings_BookingId",
                table: "foodtypes");

            migrationBuilder.DropIndex(
                name: "IX_foodtypes_BookingId",
                table: "foodtypes");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "foodtypes");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Bookings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "foodtypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_foodtypes_BookingId",
                table: "foodtypes",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_foodtypes_Bookings_BookingId",
                table: "foodtypes",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "BookingId");
        }
    }
}
