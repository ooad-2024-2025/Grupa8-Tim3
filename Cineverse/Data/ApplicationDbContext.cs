using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cineverse.Models;

namespace Cineverse.Data
{
    public class ApplicationDbContext : IdentityDbContext<Korisnik>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Rezervacija> Rezervacija { get; set; }
        public DbSet<Film> Film { get; set; }
        public DbSet<Karta> Karta { get; set; }
        public DbSet<Sjediste> Sjediste { get; set; }
        public DbSet<Projekcija> Projekcija { get; set; }
        public DbSet<Cijena> Cijena { get; set; }
        public DbSet<Dvorana> Dvorana { get; set; }
        public DbSet<PregledKarata> PregledKarata { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Korisnik>().ToTable("Korisnik");
            modelBuilder.Entity<Rezervacija>().ToTable("Rezervacija");
            modelBuilder.Entity<Film>().ToTable("Film");
            modelBuilder.Entity<Karta>().ToTable("Karta");
            modelBuilder.Entity<Sjediste>().ToTable("Sjediste");
            modelBuilder.Entity<Projekcija>().ToTable("Projekcija");
            modelBuilder.Entity<Cijena>().ToTable("Cijena");
            modelBuilder.Entity<Dvorana>().ToTable("Dvorana");
            modelBuilder.Entity<PregledKarata>().ToTable("PregledKarata");


           

            // Ostale konfiguracije...


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Projekcija>()
        .HasOne(p => p.Film)
        .WithMany()
        .HasForeignKey(p => p.FilmId)
        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rezervacija>()
        .HasOne(p => p.Projekcija)
        .WithMany()
        .HasForeignKey(p => p.ProjekcijaId)
        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
