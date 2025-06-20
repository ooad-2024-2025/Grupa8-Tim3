using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cineverse.Models
{
    public class Sjediste
    {
        [Key]
        public int Id { get; set; }
        public int Red { get; set; }
        public int Kolona { get; set; }
        [Display(Name = "Tip sjedista")]
        [EnumDataType(typeof(TipSjedista))]
       
        public TipSjedista TipSjedista
        { get; set; }

        public int DvoranaId { get; set; }

        [ForeignKey("DvoranaId")]
        public Dvorana Dvorana { get; set; }

    }
}
