using System.ComponentModel.DataAnnotations;

namespace TokoAlatTulisAPI.DTOs
{
    public class CreatePemesananDto
    {
        [Required]
        public int IdBarang { get; set; }

        [Required(ErrorMessage = "Nama pembeli wajib diisi")]
        [StringLength(100)]
        public string NamaPembeli { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Jumlah minimal 1")]
        public int Jumlah { get; set; }

        // Catatan: TotalBayar biasanya tidak dikirim dari Client, 
        // melainkan dihitung otomatis di Backend (Harga * Jumlah).
        // TanggalPemesanan juga otomatis diisi Backend.
    }
}
