using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Cineverse.Models
{
	public class Korisnik
	{
		[Key]
		public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Prezime ne smije imati više od 50 karaktera!")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Dozvoljeno je samo korištenje velikih, malih slova i razmaka!")]
        [DisplayName("Prezime:")]
        public string Prezime { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Datum rođenja:")]
        [Required(ErrorMessage = "Datum rođenja je obavezan!")]
        public DateTime DatumRodjenja { get; set; }

        [Required(ErrorMessage = "Email je obavezan!")]
        [EmailAddress(ErrorMessage = "Unesite ispravan email!")]
        [StringLength(100, ErrorMessage = "Email ne smije imati više od 100 karaktera!")]
        [DisplayName("Email:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Korisničko ime je obavezno!")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Korisničko ime mora imati između 4 i 30 karaktera!")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Dozvoljena su samo slova, brojevi i donja crta!")]
        [DisplayName("Korisničko ime:")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna!")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Lozinka mora imati najmanje 6 karaktera!")]
        [DataType(DataType.Password)]
        [DisplayName("Lozinka:")]
        public string Password { get; set; }

        [StringLength(maximumLength: 50, ErrorMessage = "Ime ne smije imati vise od 50 karaktera!")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Dozvoljeno je samo korištenje velikih, malih slova i razmaka!")]  
        [DisplayName("Ime:")]
        public string Ime { get; set; }
	}
}
