using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI_Kesim.Data.Migrations
{
    /// <inheritdoc />
    public partial class HizmetUcretEkleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ucret",
                table: "Uzmanliklar",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ucret",
                table: "Uzmanliklar");
        }
    }
}
