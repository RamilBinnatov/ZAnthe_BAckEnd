using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZAnthe_BAckEnd.Migrations
{
    public partial class updateRoomTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_roomServices_rooms_RoomId",
                table: "roomServices");

            migrationBuilder.DropIndex(
                name: "IX_roomServices_RoomId",
                table: "roomServices");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "roomServices");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

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

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "awards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Img",
                table: "awards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "roomServices",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "awards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Img",
                table: "awards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_roomServices_RoomId",
                table: "roomServices",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_roomServices_rooms_RoomId",
                table: "roomServices",
                column: "RoomId",
                principalTable: "rooms",
                principalColumn: "Id");
        }
    }
}
