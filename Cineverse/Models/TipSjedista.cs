using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cineverse.Models
{
	public enum TipSjedista
	{
        [Display(Name = "VIP Sjediste")]
        VIP,
        [Display(Name = "LOVEBOX Sjediste")]
        LOVEBOX,
        [Display(Name = "REGULAR Sjediste")]
        REGULAR
	}
}
