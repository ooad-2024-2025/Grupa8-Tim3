using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cineverse.Models
{
    public class Projekcija
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Dvorana")]
        public int DvoranaId { get; set; }
        public string Lokacija { get; set; }
        public DateTime Datum { get; set; }
        public string Vrijeme { get; set; }
        [ForeignKey("Film")]
        public int FilmId { get; set; }
    }
}
