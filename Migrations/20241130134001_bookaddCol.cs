using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eproject.Migrations
{
    /// <inheritdoc />
    public partial class bookaddCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_caterers_CatererId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CatererId",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "foodtypes",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "SelectedFoodTypeIds",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "SelectedFoodTypeIds",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerPhone",
                table: "Bookings",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                table: "Bookings",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CatererId",
                table: "Bookings",
                column: "CatererId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_caterers_CatererId",
                table: "Bookings",
                column: "CatererId",
                principalTable: "caterers",
                principalColumn: "CatererId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
