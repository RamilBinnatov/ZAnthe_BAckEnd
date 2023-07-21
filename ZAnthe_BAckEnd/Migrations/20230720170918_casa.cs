using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZAnthe_BAckEnd.Migrations
{
    public partial class casa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "BedType",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "GuestCount",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "MinBookingLim",
                table: "rooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Area",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BedType",
                table: "rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GuestCount",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinBookingLim",
                table: "rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
