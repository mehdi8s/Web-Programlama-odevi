using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI_Kesim.Data.Migrations
{
    /// <inheritdoc />
    public partial class CalisanUzmanlikAlani : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalismaSaati_Calisan_CalisanId",
                table: "CalismaSaati");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalismaSaati",
                table: "CalismaSaati");

            migrationBuilder.DropColumn(
                name: "UzmanlikAlani",
                table: "Calisan");

            migrationBuilder.RenameTable(
                name: "CalismaSaati",
                newName: "CalismaSaatleri");

            migrationBuilder.RenameIndex(
                name: "IX_CalismaSaati_CalisanId",
                table: "CalismaSaatleri",
                newName: "IX_CalismaSaatleri_CalisanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalismaSaatleri",
                table: "CalismaSaatleri",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Uzmanliklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzmanliklar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalisanUzmanliklari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalisanId = table.Column<int>(type: "int", nullable: false),
                    UzmanlikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalisanUzmanliklari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalisanUzmanliklari_Calisan_CalisanId",
                        column: x => x.CalisanId,
                        principalTable: "Calisan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalisanUzmanliklari_Uzmanliklar_UzmanlikId",
                        column: x => x.UzmanlikId,
                        principalTable: "Uzmanliklar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalisanUzmanliklari_CalisanId",
                table: "CalisanUzmanliklari",
                column: "CalisanId");

            migrationBuilder.CreateIndex(
                name: "IX_CalisanUzmanliklari_UzmanlikId",
                table: "CalisanUzmanliklari",
                column: "UzmanlikId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalismaSaatleri_Calisan_CalisanId",
                table: "CalismaSaatleri",
                column: "CalisanId",
                principalTable: "Calisan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalismaSaatleri_Calisan_CalisanId",
                table: "CalismaSaatleri");

            migrationBuilder.DropTable(
                name: "CalisanUzmanliklari");

            migrationBuilder.DropTable(
                name: "Uzmanliklar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalismaSaatleri",
                table: "CalismaSaatleri");

            migrationBuilder.RenameTable(
                name: "CalismaSaatleri",
                newName: "CalismaSaati");

            migrationBuilder.RenameIndex(
                name: "IX_CalismaSaatleri_CalisanId",
                table: "CalismaSaati",
                newName: "IX_CalismaSaati_CalisanId");

            migrationBuilder.AddColumn<string>(
                name: "UzmanlikAlani",
                table: "Calisan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalismaSaati",
                table: "CalismaSaati",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CalismaSaati_Calisan_CalisanId",
                table: "CalismaSaati",
                column: "CalisanId",
                principalTable: "Calisan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
