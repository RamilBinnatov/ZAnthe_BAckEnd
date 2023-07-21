﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZAnthe_BAckEnd.Migrations
{
    public partial class updateAboutTable4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "aboutBanners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "aboutBanners",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "aboutBanners");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "aboutBanners");
        }
    }
}
