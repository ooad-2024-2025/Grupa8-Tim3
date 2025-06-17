using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cineverse.Models
{

    public class PregledKarata
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "QR Kod")]
        public string QRKod { get; set; } = string.Empty;

        public string KorisnikId { get; set; }

        [ForeignKey("KorisnikId")]
        public Korisnik Korisnik { get; set; }

        /*[ForeignKey("Karta")]
        public int KartaId { get; set; }*/
    }
}
