using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cineverse.Models
{
    public class Karta
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Rezervacija")]
        public int RezervacijaId { get; set; }

        [ForeignKey("Sjediste")]
        public int SjedisteId { get; set; }
    }
}
