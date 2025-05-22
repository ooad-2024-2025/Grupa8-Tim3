using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cineverse.Models
{
    public class Rezervacija
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Projekcija")]
        public int ProjekcijaId { get; set; }
        public string Status { get; set; } = string.Empty;

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }

        [ForeignKey("Cijena")]
        public int CijenaId { get; set; }
    }

}
