using Microsoft.EntityFrameworkCore;
using TokoAlatTulisAPI.DTOs;
using TokoAlatTulisAPI.Models;

namespace TokoAlatTulisAPI.Services
{
    public class BarangService : IBarangService
    {
        private readonly AppDbContext _context;

        public BarangService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BarangDto>> GetAllBarangAsync()
        {
            var barangs = await _context.Barang.ToListAsync();

            // Manual Mapping Entity -> DTO
            return barangs.Select(b => new BarangDto
            {
                IdBarang = b.IdBarang,
                NamaBarang = b.NamaBarang,
                Harga = b.Harga,
                Stok = b.Stok,
                Kategori = b.Kategori,
                Deskripsi = b.Deskripsi
            });
        }

        public async Task<BarangDto?> GetBarangByIdAsync(int id)
        {
            var b = await _context.Barang.FindAsync(id);
            if (b == null) return null;

            return new BarangDto
            {
                IdBarang = b.IdBarang,
                NamaBarang = b.NamaBarang,
                Harga = b.Harga,
                Stok = b.Stok,
                Kategori = b.Kategori,
                Deskripsi = b.Deskripsi
            };
        }

        public async Task<BarangDto> CreateBarangAsync(CreateBarangDto createDto)
        {
            var barang = new Barang
            {
                NamaBarang = createDto.NamaBarang,
                Harga = createDto.Harga,
                Stok = createDto.Stok,
                Kategori = createDto.Kategori,
                Deskripsi = createDto.Deskripsi
            };

            _context.Barang.Add(barang);
            await _context.SaveChangesAsync();

            // Return DTO dengan ID baru
            return new BarangDto
            {
                IdBarang = barang.IdBarang,
                NamaBarang = barang.NamaBarang,
                Harga = barang.Harga,
                Stok = barang.Stok,
                Kategori = barang.Kategori,
                Deskripsi = barang.Deskripsi
            };
        }

        public async Task<bool> UpdateBarangAsync(int id, CreateBarangDto updateDto)
        {
            var existingBarang = await _context.Barang.FindAsync(id);
            if (existingBarang == null) return false;

            existingBarang.NamaBarang = updateDto.NamaBarang;
            existingBarang.Harga = updateDto.Harga;
            existingBarang.Stok = updateDto.Stok;
            existingBarang.Kategori = updateDto.Kategori;
            existingBarang.Deskripsi = updateDto.Deskripsi;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBarangAsync(int id)
        {
            var barang = await _context.Barang.FindAsync(id);
            if (barang == null) return false;

            _context.Barang.Remove(barang);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
