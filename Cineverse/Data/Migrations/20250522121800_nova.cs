using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cineverse.Data.Migrations
{
    /// <inheritdoc />
    public partial class nova : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
