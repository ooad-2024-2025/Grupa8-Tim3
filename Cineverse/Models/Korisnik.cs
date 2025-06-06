using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Cineverse.Models
{
    public class Korisnik : IdentityUser
    {
        //[Required(ErrorMessage = "Ime je obavezno!")]
        [StringLength(50, ErrorMessage = "Ime ne smije imati više od 50 karaktera!")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Dozvoljeno je samo korištenje velikih, malih slova i razmaka!")]
        [DisplayName("Ime:")]
        public string? Ime { get; set; }

        //[Required(ErrorMessage = "Prezime je obavezno!")]
        [StringLength(50, ErrorMessage = "Prezime ne smije imati više od 50 karaktera!")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Dozvoljeno je samo korištenje velikih, malih slova i razmaka!")]
        [DisplayName("Prezime:")]
        public string? Prezime { get; set; }

        [ValidateDate]
        [DataType(DataType.Date)]
        [DisplayName("Datum rođenja:")]
        //[Required(ErrorMessage = "Datum rođenja je obavezan!")]
        public DateTime? DatumRodjenja { get; set; }

        // Email se nasleđuje od IdentityUser - ne override-ujemo ga
        // Možete dodati dodatne validacije u Register akciji ili DTO
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