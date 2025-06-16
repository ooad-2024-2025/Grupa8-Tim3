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
        public string Lokacija { get; set; } = string.Empty;
        public DateOnly Datum { get; set; }
        public TimeOnly Vrijeme { get; set; }

        public int FilmId { get; set; }
<<<<<<< Updated upstream
=======

        [ForeignKey("FilmId")]
        public Film Film { get; set; }

     
>>>>>>> Stashed changes
    }
}
