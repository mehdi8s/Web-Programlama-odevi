using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI_Kesim.Data.Migrations
{
    /// <inheritdoc />
    public partial class RandevuTarihiEkleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaatAraligi",
                table: "CalismaSaatleri");

            migrationBuilder.AddColumn<DateTime>(
                name: "Tarih",
                table: "CalismaSaatleri",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tarih",
                table: "CalismaSaatleri");

            migrationBuilder.AddColumn<string>(
                name: "SaatAraligi",
                table: "CalismaSaatleri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
