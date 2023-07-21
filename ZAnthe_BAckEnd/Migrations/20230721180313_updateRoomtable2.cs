using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZAnthe_BAckEnd.Migrations
{
    public partial class updateRoomtable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "rooms",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "rooms");
        }
    }
}
