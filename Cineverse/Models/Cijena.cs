using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cineverse.Models
{
    public class Cijena
    {
        [Key]
        public int Id { get; set; }

        public double OsnovnaCijena { get; set; }
        public double Popust { get; set; }

        // Prazan konstruktor za EF Core
        public Cijena() { }

        public Cijena(double osnovnaCijena, double popust)
        {
            OsnovnaCijena = osnovnaCijena;
            Popust = popust;
        }
    }
}