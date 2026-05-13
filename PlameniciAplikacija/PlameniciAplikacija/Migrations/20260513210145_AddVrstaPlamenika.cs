using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlameniciAplikacija.Migrations
{
    /// <inheritdoc />
    public partial class AddVrstaPlamenika : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VrstaPlamenikaId",
                table: "Projekti",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VrstePlamenika",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VrstePlamenika", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projekti_VrstaPlamenikaId",
                table: "Projekti",
                column: "VrstaPlamenikaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projekti_VrstePlamenika_VrstaPlamenikaId",
                table: "Projekti",
                column: "VrstaPlamenikaId",
                principalTable: "VrstePlamenika",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projekti_VrstePlamenika_VrstaPlamenikaId",
                table: "Projekti");

            migrationBuilder.DropTable(
                name: "VrstePlamenika");

            migrationBuilder.DropIndex(
                name: "IX_Projekti_VrstaPlamenikaId",
                table: "Projekti");

            migrationBuilder.DropColumn(
                name: "VrstaPlamenikaId",
                table: "Projekti");
        }
    }
}
