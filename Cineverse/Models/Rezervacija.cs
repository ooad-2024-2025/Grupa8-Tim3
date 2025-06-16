using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cineverse.Models
{
    public class Rezervacija
    {
        [Key]
        public int Id { get; set; }
        public int ProjekcijaId { get; set; }
        [ForeignKey("ProjekcijaId")]
        public Projekcija Projekcija { get; set; }
        public string Status { get; set; } = string.Empty;

        [ForeignKey("Cijena")]
        public int CijenaId { get; set; }

        public string KorisnikId { get; set; }

        [ForeignKey("KorisnikId")]
        public Korisnik Korisnik { get; set; }

    }

}
