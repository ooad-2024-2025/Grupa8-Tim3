using System.ComponentModel.DataAnnotations;

namespace Cineverse.Models
{
    public class Cijena
    {
        [Key]
        public int Id { get; set; }
        public double OsnovnaCijena { get; set; }
        public double Popust { get; set; }
    }
}
