using System;
using System.Collections.Generic;
using System.Linq;
using Cineverse.Data;

namespace Cineverse.Models
{
    public class RepertoarSingleton
    {
        private static RepertoarSingleton _instanca;
        private static readonly object _lock = new object();
        private List<Film> _filmovi;

        
        private RepertoarSingleton(ApplicationDbContext context)
        {
            _filmovi = context.Film.ToList();
        }

        public static RepertoarSingleton GetInstance(ApplicationDbContext context)
        {
            if (_instanca == null)
            {
                lock (_lock)
                {
                    if (_instanca == null)
                    {
                        _instanca = new RepertoarSingleton(context);
                    }
                }
            }
            return _instanca;
        }

        public List<Film> GetFilmovi()
        {
            return _filmovi;
        }
    }
}