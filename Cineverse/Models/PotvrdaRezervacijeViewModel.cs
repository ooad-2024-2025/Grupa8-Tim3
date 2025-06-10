namespace Cineverse.Models
{

    public class PotvrdaRezervacijeViewModel
    {
        public string Film { get; set; }
        public string Dvorana { get; set; }
        public List<SjedisteInfo> Sjedista { get; set; }
        public decimal OsnovnaCijena { get; set; }
        public decimal Popust { get; set; }
        public decimal UkupnaCijena { get; set; }
        public int BrojSjedista { get; set; }
    }

    public class SjedisteInfo
    {
        public int Red { get; set; }
        public int Kolona { get; set; }
    }
}
