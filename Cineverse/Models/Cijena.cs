using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cineverse.Models
{
    public class Cijena
    {
        [Key]
        public int Id { get; set; }
        public double OsnovnaCijena { get; set; }

        [ForeignKey("Film")]
        public int FilmId { get; set; }
    }
}
