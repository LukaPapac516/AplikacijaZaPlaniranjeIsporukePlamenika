using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlameniciAplikacija.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Djelatnici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ime = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Prezime = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RadnoMjesto = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Tip = table.Column<int>(type: "INTEGER", nullable: false),
                    DatumZaposlenja = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Djelatnici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kupci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    OIB = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false),
                    Adresa = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Grad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    KontaktOsoba = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Telefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Tip = table.Column<int>(type: "INTEGER", nullable: false),
                    DatumUnosa = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kupci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projekti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BrojProjekta = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    KupacId = table.Column<int>(type: "INTEGER", nullable: true),
                    DatumUnosa = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StatusPripremeRada = table.Column<int>(type: "INTEGER", nullable: false),
                    StatusLansiranja = table.Column<int>(type: "INTEGER", nullable: false),
                    StatusProizvodnje = table.Column<int>(type: "INTEGER", nullable: false),
                    OcekivaniRokIsporuke = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RealniRokIsporuke = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Napomena = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    NapomenaPrioritet = table.Column<int>(type: "INTEGER", nullable: false),
                    Prioritet = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projekti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projekti_Kupci_KupacId",
                        column: x => x.KupacId,
                        principalTable: "Kupci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DjelatnikProject",
                columns: table => new
                {
                    DjelatniciId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProjektiId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DjelatnikProject", x => new { x.DjelatniciId, x.ProjektiId });
                    table.ForeignKey(
                        name: "FK_DjelatnikProject_Djelatnici_DjelatniciId",
                        column: x => x.DjelatniciId,
                        principalTable: "Djelatnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DjelatnikProject_Projekti_ProjektiId",
                        column: x => x.ProjektiId,
                        principalTable: "Projekti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FazeProjekta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Redosljed = table.Column<int>(type: "INTEGER", nullable: false),
                    PlaniraniPocetak = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PlaniraniZavrsetak = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StvarniPocetak = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StvarniZavrsetak = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Napomena = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    DatumPlaniranja = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProjektId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FazeProjekta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FazeProjekta_Projekti_ProjektId",
                        column: x => x.ProjektId,
                        principalTable: "Projekti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Napomene",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tekst = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    DatumUnosa = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AutorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Prioritet = table.Column<int>(type: "INTEGER", nullable: false),
                    DatumOtvaranja = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Napomene", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Napomene_Djelatnici_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Djelatnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Napomene_Projekti_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projekti",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RadniNalozi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OznakaNaloga = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DatumOtvaranja = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DatumZatvaranja = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Opis = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ProjektId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadniNalozi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RadniNalozi_Projekti_ProjektId",
                        column: x => x.ProjektId,
                        principalTable: "Projekti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StavkeProizvodnje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tip = table.Column<int>(type: "INTEGER", nullable: false),
                    DatumPlaniranja = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DatumZavrsetka = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Napomena = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ProjektId = table.Column<int>(type: "INTEGER", nullable: false),
                    RadniNalogId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StavkeProizvodnje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StavkeProizvodnje_Projekti_ProjektId",
                        column: x => x.ProjektId,
                        principalTable: "Projekti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StavkeProizvodnje_RadniNalozi_RadniNalogId",
                        column: x => x.RadniNalogId,
                        principalTable: "RadniNalozi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DjelatnikProject_ProjektiId",
                table: "DjelatnikProject",
                column: "ProjektiId");

            migrationBuilder.CreateIndex(
                name: "IX_FazeProjekta_ProjektId",
                table: "FazeProjekta",
                column: "ProjektId");

            migrationBuilder.CreateIndex(
                name: "IX_Napomene_AutorId",
                table: "Napomene",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Napomene_ProjectId",
                table: "Napomene",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projekti_KupacId",
                table: "Projekti",
                column: "KupacId");

            migrationBuilder.CreateIndex(
                name: "IX_RadniNalozi_ProjektId",
                table: "RadniNalozi",
                column: "ProjektId");

            migrationBuilder.CreateIndex(
                name: "IX_StavkeProizvodnje_ProjektId",
                table: "StavkeProizvodnje",
                column: "ProjektId");

            migrationBuilder.CreateIndex(
                name: "IX_StavkeProizvodnje_RadniNalogId",
                table: "StavkeProizvodnje",
                column: "RadniNalogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DjelatnikProject");

            migrationBuilder.DropTable(
                name: "FazeProjekta");

            migrationBuilder.DropTable(
                name: "Napomene");

            migrationBuilder.DropTable(
                name: "StavkeProizvodnje");

            migrationBuilder.DropTable(
                name: "Djelatnici");

            migrationBuilder.DropTable(
                name: "RadniNalozi");

            migrationBuilder.DropTable(
                name: "Projekti");

            migrationBuilder.DropTable(
                name: "Kupci");
        }
    }
}
