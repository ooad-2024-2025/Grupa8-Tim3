namespace Cineverse.Models
{
    public class PregledKarataViewModel
    {
        public string QRKod { get; set; }
        public string NazivFilma { get; set; }
        public string SlikaFilmaUrl { get; set; }
        public TimeOnly VrijemeProjekcije { get; set; }
        
        public DateOnly DatumProjekcije { get; set; }
        public string Sala { get; set; }
        public string Red { get; set; }
        public string Sjediste { get; set; }
        public double Iznos { get; set; }
        public string Lokacija { get; set; }
    }


}
