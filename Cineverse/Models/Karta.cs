using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cineverse.Models
{
    public class Karta
    {
        [Key]
        public int Id { get; set; }

        public int RezervacijaId { get; set; }


        [ForeignKey("RezervacijaId")]
        public Rezervacija Rezervacija { get; set; }

        public int SjedisteId { get; set; }

        [ForeignKey("SjedisteId")]

        public Sjediste Sjediste { get; set; }
    }
}
