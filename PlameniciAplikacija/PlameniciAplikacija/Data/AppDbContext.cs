using Microsoft.EntityFrameworkCore;
using PlameniciAplikacija.Models;

namespace PlameniciAplikacija.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet svojstva za sve modele
        public DbSet<Project> Projekti { get; set; }
        public DbSet<Kupac> Kupci { get; set; }
        public DbSet<Djelatnik> Djelatnici { get; set; }
        public DbSet<RadniNalog> RadniNalozi { get; set; }
        public DbSet<StavkaProizvodnje> StavkeProizvodnje { get; set; }
        public DbSet<FazaProjekta> FazeProjekta { get; set; }
        public DbSet<Napomena> Napomene { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracija N:M relacije između Project i Djelatnik
            // EF Core će automatski kreirati junction tabelu "DjelatnikProject"
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Djelatnici)
                .WithMany(d => d.Projekti);

            // Konfiguracija One-to-Many: Kupac -> Project
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Kupac)
                .WithMany(k => k.Projekti)
                .HasForeignKey(p => p.KupacId)
                .OnDelete(DeleteBehavior.SetNull);

            // Konfiguracija One-to-Many: Project -> RadniNalog
            modelBuilder.Entity<RadniNalog>()
                .HasOne(rn => rn.Projekt)
                .WithMany(p => p.RadniNalozi)
                .HasForeignKey(rn => rn.ProjektId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracija One-to-Many: Project -> StavkaProizvodnje
            modelBuilder.Entity<StavkaProizvodnje>()
                .HasOne(sp => sp.Projekt)
                .WithMany(p => p.StavkeProizvodnje)
                .HasForeignKey(sp => sp.ProjektId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracija One-to-Many: RadniNalog -> StavkaProizvodnje
            modelBuilder.Entity<StavkaProizvodnje>()
                .HasOne(sp => sp.RadniNalog)
                .WithMany(rn => rn.Operacije)
                .HasForeignKey(sp => sp.RadniNalogId)
                .OnDelete(DeleteBehavior.SetNull);

            // Konfiguracija One-to-Many: Project -> FazaProjekta
            modelBuilder.Entity<FazaProjekta>()
                .HasOne(fp => fp.Projekt)
                .WithMany(p => p.FazeProjekta)
                .HasForeignKey(fp => fp.ProjektId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracija One-to-Many: Djelatnik -> Napomena (kao Autor)
            modelBuilder.Entity<Napomena>()
                .HasOne(n => n.Autor)
                .WithMany()
                .HasForeignKey(n => n.AutorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Konfiguracija One-to-Many: Project -> Napomena
            // Napomena je povezana sa Projektom preko StavkeProizvodnje
            // Ako trebamo direktnu veza, trebamo dodati ProjectId u Napomena
        }
    }
}
