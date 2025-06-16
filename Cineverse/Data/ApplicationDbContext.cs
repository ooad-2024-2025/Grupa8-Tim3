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

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Projekcija>()
        .HasOne(p => p.Film)
        .WithMany()
        .HasForeignKey(p => p.FilmId)
        .OnDelete(DeleteBehavior.Cascade);
<<<<<<< Updated upstream

            modelBuilder.Entity<Rezervacija>()
        .HasOne(p => p.Projekcija)
        .WithMany()
        .HasForeignKey(p => p.ProjekcijaId)
        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Karta>()
        .HasOne(p => p.Rezervacija)
        .WithMany()
        .HasForeignKey(p => p.RezervacijaId)
        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sjediste>()
        .HasOne(p => p.Dvorana)
        .WithMany()
        .HasForeignKey(p => p.DvoranaId)
        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PregledKarata>()
    .HasOne(p => p.Korisnik)
    .WithMany()
    .HasForeignKey(p => p.KorisnikId)
    .OnDelete(DeleteBehavior.Cascade);

=======
>>>>>>> Stashed changes
        }
    }
}
