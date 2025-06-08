using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Cineverse.Models
{
    public class Korisnik : IdentityUser
    {
        [DisplayName("Ime:")]
        public string? Ime { get; set; }

        [DisplayName("Prezime:")]
        public string? Prezime { get; set; }

        [DisplayName("Datum rođenja:")]
        public DateTime? DatumRodjenja { get; set; }

        // Override the inherited properties to make them nullable if needed
        public override string? PhoneNumber { get; set; }

        // If you're having issues with other inherited properties, you might need to override them too
        // But be careful as this can affect Identity functionality
    }
}

public class ValidateDate : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // Allow null values
        }

        if (!(value is DateTime datumRodjenja))
        {
            return new ValidationResult("Neispravan datum rođenja.");
        }

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