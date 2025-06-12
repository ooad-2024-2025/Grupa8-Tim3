using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cineverse.Models
{

    public class PregledKarata
    {
        [Key]
        public int Id { get; set; }
        public string QRKod { get; set; } = string.Empty;

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }

        [ForeignKey("Karta")]
        public int KartaId { get; set; }
    }
}
