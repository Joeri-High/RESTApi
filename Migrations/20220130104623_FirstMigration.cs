using Microsoft.EntityFrameworkCore.Migrations;

namespace RESTApi.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artikels",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PotMaat = table.Column<int>(type: "int", nullable: false),
                    PlantHoogte = table.Column<int>(type: "int", nullable: false),
                    kleur = table.Column<int>(type: "int", nullable: false),
                    productGroep = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artikels", x => x.Code);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artikels");
        }
    }
}
