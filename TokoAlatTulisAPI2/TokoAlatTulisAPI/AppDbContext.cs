using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TokoAlatTulisAPI.Models;

namespace TokoAlatTulisAPI
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Barang> Barang { get; set; }
        public DbSet<Pemesanan> Pemesanan { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Konfigurasi Barang
            modelBuilder.Entity<Barang>(entity =>
            {
                // Set default value 0 untuk Stok
                entity.Property(e => e.Stok)
                      .HasDefaultValue(0);

                // Opsional: Untuk MySQL tipe Decimal harus presisi
                entity.Property(e => e.Harga)
                      .HasColumnType("decimal(10,2)");
            });

            // 2. Konfigurasi Pemesanan
            modelBuilder.Entity<Pemesanan>(entity =>
            {
                // Set default tanggal saat ini (Syntax MySQL)
                entity.Property(e => e.TanggalPemesanan)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.TotalBayar)
                      .HasColumnType("decimal(10,2)");

                // Konfigurasi Foreign Key
                entity.HasOne(d => d.Barang)
                      .WithMany(p => p.ListPemesanan)
                      .HasForeignKey(d => d.IdBarang)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
