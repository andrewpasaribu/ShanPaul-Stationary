using Microsoft.EntityFrameworkCore;
using TokoAlatTulisAPI.DTOs;
using TokoAlatTulisAPI.Models;

namespace TokoAlatTulisAPI.Services
{
    public class PemesananService : IPemesananService
    {
        private readonly AppDbContext _context;

        public PemesananService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PemesananDto>> GetAllPemesananAsync()
        {
            // Include untuk mengambil Nama Barang dari tabel relasi
            var list = await _context.Pemesanan
                .Include(p => p.Barang)
                .OrderByDescending(p => p.TanggalPemesanan)
                .ToListAsync();

            return list.Select(p => new PemesananDto
            {
                IdPemesanan = p.IdPemesanan,
                IdBarang = p.IdBarang,
                NamaBarang = p.Barang?.NamaBarang ?? "Barang Dihapus/Tidak Diketahui",
                NamaPembeli = p.NamaPembeli,
                Jumlah = p.Jumlah,
                TotalBayar = p.TotalBayar,
                TanggalPemesanan = p.TanggalPemesanan
            });
        }

        public async Task<PemesananDto?> GetPemesananByIdAsync(int id)
        {
            var p = await _context.Pemesanan
                .Include(b => b.Barang)
                .FirstOrDefaultAsync(x => x.IdPemesanan == id);

            if (p == null) return null;

            return new PemesananDto
            {
                IdPemesanan = p.IdPemesanan,
                IdBarang = p.IdBarang,
                NamaBarang = p.Barang?.NamaBarang,
                NamaPembeli = p.NamaPembeli,
                Jumlah = p.Jumlah,
                TotalBayar = p.TotalBayar,
                TanggalPemesanan = p.TanggalPemesanan
            };
        }

        public async Task<(PemesananDto? result, string? errorMessage)> CreatePemesananAsync(CreatePemesananDto createDto)
        {
            // 1. Cek apakah barang ada
            var barang = await _context.Barang.FindAsync(createDto.IdBarang);
            if (barang == null)
            {
                return (null, "Barang tidak ditemukan.");
            }

            // 2. Cek stok
            if (barang.Stok < createDto.Jumlah)
            {
                return (null, $"Stok tidak cukup. Sisa stok: {barang.Stok}");
            }

            // 3. Hitung Total Bayar
            decimal totalBayar = barang.Harga * createDto.Jumlah;

            // 4. Kurangi Stok Barang
            barang.Stok -= createDto.Jumlah;

            // 5. Buat Entitas Pemesanan
            var pemesanan = new Pemesanan
            {
                IdBarang = createDto.IdBarang,
                NamaPembeli = createDto.NamaPembeli,
                Jumlah = createDto.Jumlah,
                TotalBayar = totalBayar,
                TanggalPemesanan = DateTime.Now
            };

            _context.Pemesanan.Add(pemesanan);

            // Simpan perubahan (Update stok barang & Insert pemesanan terjadi dalam 1 transaksi DB)
            await _context.SaveChangesAsync();

            // Return DTO
            var resultDto = new PemesananDto
            {
                IdPemesanan = pemesanan.IdPemesanan,
                IdBarang = pemesanan.IdBarang,
                NamaBarang = barang.NamaBarang,
                NamaPembeli = pemesanan.NamaPembeli,
                Jumlah = pemesanan.Jumlah,
                TotalBayar = pemesanan.TotalBayar,
                TanggalPemesanan = pemesanan.TanggalPemesanan
            };

            return (resultDto, null);
        }

        public async Task<bool> DeletePemesananAsync(int id)
        {
            var pemesanan = await _context.Pemesanan.FindAsync(id);
            if (pemesanan == null) return false;

            // Optional: Kembalikan Stok ke Barang saat pesanan dihapus
            if (pemesanan.IdBarang != null)
            {
                var barang = await _context.Barang.FindAsync(pemesanan.IdBarang);
                if (barang != null)
                {
                    barang.Stok += pemesanan.Jumlah;
                }
            }

            _context.Pemesanan.Remove(pemesanan);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
