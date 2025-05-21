using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cineverse.Models
{
	public class Korisnik
	{
		[Key]
		public int Id { get; set; }
		public string Prezime { get; set; }
		public DateTime DatumRodjenja { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Ime { get; set; }
	}
}
