using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cineverse.Data.Migrations
{
    /// <inheritdoc />
    public partial class kartamig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KorisnikId",
                table: "PregledKarata",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Sjediste_DvoranaId",
                table: "Sjediste",
                column: "DvoranaId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Cijena_FilmId",
                table: "Cijena",
                column: "FilmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cijena_Film_FilmId",
                table: "Cijena",
                column: "FilmId",
                principalTable: "Film",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_PregledKarata_AspNetUsers_KorisnikId",
                table: "PregledKarata",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
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
                name: "FK_Cijena_Film_FilmId",
                table: "Cijena");

            migrationBuilder.DropForeignKey(
                name: "FK_Karta_Rezervacija_RezervacijaId",
                table: "Karta");

            migrationBuilder.DropForeignKey(
                name: "FK_Karta_Sjediste_SjedisteId",
                table: "Karta");

            migrationBuilder.DropForeignKey(
                name: "FK_PregledKarata_AspNetUsers_KorisnikId",
                table: "PregledKarata");

            migrationBuilder.DropForeignKey(
                name: "FK_Sjediste_Dvorana_DvoranaId",
                table: "Sjediste");

            migrationBuilder.DropIndex(
                name: "IX_Sjediste_DvoranaId",
                table: "Sjediste");

            migrationBuilder.DropIndex(
                name: "IX_PregledKarata_KorisnikId",
                table: "PregledKarata");

            migrationBuilder.DropIndex(
                name: "IX_Karta_RezervacijaId",
                table: "Karta");

            migrationBuilder.DropIndex(
                name: "IX_Karta_SjedisteId",
                table: "Karta");

            migrationBuilder.DropIndex(
                name: "IX_Cijena_FilmId",
                table: "Cijena");

            migrationBuilder.AlterColumn<string>(
                name: "KorisnikId",
                table: "PregledKarata",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
