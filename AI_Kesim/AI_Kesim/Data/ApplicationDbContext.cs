using AI_Kesim.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AI_Kesim.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserDetails>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Calisan> Calisan { get; set; }
        public DbSet<Uzmanlik> Uzmanliklar { get; set; }
        public DbSet<CalisanUzmanlik> CalisanUzmanliklari { get; set; }
        public DbSet<Randevu> Randevular { get; set; } // Randevu tablosu

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Çalışan-Uzmanlık ilişkisi için ara tablo konfigürasyonu
            modelBuilder.Entity<CalisanUzmanlik>()
                .HasOne(cu => cu.Calisan)
                .WithMany(c => c.CalisanUzmanliklari)
                .HasForeignKey(cu => cu.CalisanId);

            modelBuilder.Entity<CalisanUzmanlik>()
                .HasOne(cu => cu.Uzmanlik)
                .WithMany(u => u.CalisanUzmanliklari)
                .HasForeignKey(cu => cu.UzmanlikId);

            // Randevu ilişkileri
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Uzmanlik)
                .WithMany()
                .HasForeignKey(r => r.UzmanlikId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Calisan)
                .WithMany()
                .HasForeignKey(r => r.CalisanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}