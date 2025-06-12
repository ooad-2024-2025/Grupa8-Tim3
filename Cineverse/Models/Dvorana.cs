using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cineverse.Models
{

	public class Dvorana
	{
		[Key]
		public int Id { get; set; }
		public string NazivDvorane { get; set; }
		public int Kapacitet { get; set; }
		//public int SlobodnaMjesta { get; set; }
	}
}
