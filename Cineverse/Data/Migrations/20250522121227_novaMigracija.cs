using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cineverse.Data.Migrations
{
    /// <inheritdoc />
    public partial class novaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_Korisnik_KorisnikId",
                table: "Rezervacija");

            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_Projekcija_ProjekcijaId",
                table: "Rezervacija");

            migrationBuilder.DropIndex(
                name: "IX_Rezervacija_KorisnikId",
                table: "Rezervacija");

            migrationBuilder.DropIndex(
                name: "IX_Rezervacija_ProjekcijaId",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "OsnovnaCijena",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "Popust",
                table: "Rezervacija");

            migrationBuilder.AddColumn<int>(
                name: "CijenaId",
                table: "Rezervacija",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sjediste_DvoranaId",
                table: "Sjediste",
                column: "DvoranaId");

            migrationBuilder.CreateIndex(
                name: "IX_Projekcija_DvoranaId",
                table: "Projekcija",
                column: "DvoranaId");

            migrationBuilder.CreateIndex(
                name: "IX_Projekcija_FilmId",
                table: "Projekcija",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_PregledKarata_KorisnikId",
                table: "PregledKarata",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Karta_RezervacijaId",
                table: "Karta",
                column: "RezervacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Karta_SjedisteId",
                table: "Karta",
                column: "SjedisteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Karta_Rezervacija_RezervacijaId",
                table: "Karta",
                column: "RezervacijaId",
                principalTable: "Rezervacija",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Karta_Sjediste_SjedisteId",
                table: "Karta",
                column: "SjedisteId",
                principalTable: "Sjediste",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PregledKarata_Korisnik_KorisnikId",
                table: "PregledKarata",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projekcija_Dvorana_DvoranaId",
                table: "Projekcija",
                column: "DvoranaId",
                principalTable: "Dvorana",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projekcija_Film_FilmId",
                table: "Projekcija",
                column: "FilmId",
                principalTable: "Film",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sjediste_Dvorana_DvoranaId",
                table: "Sjediste",
                column: "DvoranaId",
                principalTable: "Dvorana",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Karta_Rezervacija_RezervacijaId",
                table: "Karta");

            migrationBuilder.DropForeignKey(
                name: "FK_Karta_Sjediste_SjedisteId",
                table: "Karta");

            migrationBuilder.DropForeignKey(
                name: "FK_PregledKarata_Korisnik_KorisnikId",
                table: "PregledKarata");

            migrationBuilder.DropForeignKey(
                name: "FK_Projekcija_Dvorana_DvoranaId",
                table: "Projekcija");

            migrationBuilder.DropForeignKey(
                name: "FK_Projekcija_Film_FilmId",
                table: "Projekcija");

            migrationBuilder.DropForeignKey(
                name: "FK_Sjediste_Dvorana_DvoranaId",
                table: "Sjediste");

            migrationBuilder.DropIndex(
                name: "IX_Sjediste_DvoranaId",
                table: "Sjediste");

            migrationBuilder.DropIndex(
                name: "IX_Projekcija_DvoranaId",
                table: "Projekcija");

            migrationBuilder.DropIndex(
                name: "IX_Projekcija_FilmId",
                table: "Projekcija");

            migrationBuilder.DropIndex(
                name: "IX_PregledKarata_KorisnikId",
                table: "PregledKarata");

            migrationBuilder.DropIndex(
                name: "IX_Karta_RezervacijaId",
                table: "Karta");

            migrationBuilder.DropIndex(
                name: "IX_Karta_SjedisteId",
                table: "Karta");

            migrationBuilder.DropColumn(
                name: "CijenaId",
                table: "Rezervacija");

            migrationBuilder.AddColumn<double>(
                name: "OsnovnaCijena",
                table: "Rezervacija",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Popust",
                table: "Rezervacija",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_KorisnikId",
                table: "Rezervacija",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_ProjekcijaId",
                table: "Rezervacija",
                column: "ProjekcijaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_Korisnik_KorisnikId",
                table: "Rezervacija",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_Projekcija_ProjekcijaId",
                table: "Rezervacija",
                column: "ProjekcijaId",
                principalTable: "Projekcija",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
