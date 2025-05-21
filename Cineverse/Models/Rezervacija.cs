using System;
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
        public Projekcija Projekcija { get; set; }

        public string Status { get; set; }

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        public double OsnovnaCijena { get; set; }
        public double Popust { get; set; }

        public Rezervacija() { }
    }
}
