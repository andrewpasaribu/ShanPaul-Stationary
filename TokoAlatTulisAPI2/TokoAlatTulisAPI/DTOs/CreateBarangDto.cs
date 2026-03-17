using System.ComponentModel.DataAnnotations;

namespace TokoAlatTulisAPI.DTOs
{
    public class CreateBarangDto
    {
        [Required(ErrorMessage = "Nama barang wajib diisi")]
        [StringLength(100)]
        public string NamaBarang { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Harga tidak boleh negatif")]
        public decimal Harga { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stok tidak boleh negatif")]
        public int Stok { get; set; }

        [StringLength(50)]
        public string? Kategori { get; set; }

        public string? Deskripsi { get; set; }
    }
}
