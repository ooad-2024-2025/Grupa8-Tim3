using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
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

        [ValidateDate]
        [DataType(DataType.Date)]
        [DisplayName("Datum rođenja:")]
        [Required(ErrorMessage = "Datum rođenja je obavezan!")]
        public DateTime DatumRodjenja { get; set; }

        [DisplayName("Email:")]
        [Required(ErrorMessage = "Email je obavezan!")]
        [StringLength(100, ErrorMessage = "Email ne smije imati više od 100 karaktera!")]
        [EmailAddress(ErrorMessage = "Unesite ispravan email!")]
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
public class ValidateDate : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || !(value is DateTime))
        {
            return new ValidationResult("Neispravan datum rođenja.");
        }

        DateTime datumRodjenja = (DateTime)value;
        DateTime danas = DateTime.Today;
        int godine = danas.Year - datumRodjenja.Year;

        // Ako korisnik još nije imao rođendan ove godine
        if (datumRodjenja.Date > danas.AddYears(-godine))
        {
            godine--;
        }

        if (godine < 12 || godine > 100)
        {
            return new ValidationResult("Dozvoljeno je samo za korisnike između 12 i 100 godina.");
        }

        return ValidationResult.Success;
    }
}
