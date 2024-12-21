using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI_Kesim.Data.Migrations
{
    /// <inheritdoc />
    public partial class CalisanlarEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calisan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Soyisim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UzmanlikAlani = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Maas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calisan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalismaSaati",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalisanId = table.Column<int>(type: "int", nullable: false),
                    SaatAraligi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalismaSaati", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalismaSaati_Calisan_CalisanId",
                        column: x => x.CalisanId,
                        principalTable: "Calisan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalismaSaati_CalisanId",
                table: "CalismaSaati",
                column: "CalisanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalismaSaati");

            migrationBuilder.DropTable(
                name: "Calisan");
        }
    }
}
