using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cineverse.Models
{
    public class Film
    {
        [Key]
        public int Id { get; set; }
        public string Zanr { get; set; }
        public string VrijemeTrajanja { get; set; }
        public string Uloge { get; set; }
        public string Sinopsis { get; set; }
        public string Reziser { get; set; }
        public string Trailer { get; set; }
        public string NazivFilma { get; set; }
        public string Poster { get; set; }
    }
}
