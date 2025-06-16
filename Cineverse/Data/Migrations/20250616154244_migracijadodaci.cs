using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cineverse.Data.Migrations
{
    /// <inheritdoc />
    public partial class migracijadodaci : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KorisnikId",
                table: "Rezervacija",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_KorisnikId",
                table: "Rezervacija",
                column: "KorisnikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_AspNetUsers_KorisnikId",
                table: "Rezervacija",
                column: "KorisnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_AspNetUsers_KorisnikId",
                table: "Rezervacija");

            migrationBuilder.DropIndex(
                name: "IX_Rezervacija_KorisnikId",
                table: "Rezervacija");

            migrationBuilder.AlterColumn<string>(
                name: "KorisnikId",
                table: "Rezervacija",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
