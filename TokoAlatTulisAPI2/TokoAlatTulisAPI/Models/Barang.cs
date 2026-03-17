using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokoAlatTulisAPI.Models
{
    [Table("barang")]
    public class Barang
    {
        [Key]
        [Column("id_barang")]
        public int IdBarang { get; set; }

        [Required]
        [Column("nama_barang")]
        [StringLength(100)]
        public string NamaBarang { get; set; } = string.Empty;

        [Required]
        [Column("harga", TypeName = "decimal(10, 2)")]
        public decimal Harga { get; set; }

        [Required]
        [Column("stok")]
        public int Stok { get; set; } = 0;

        [Column("kategori")]
        [StringLength(50)]
        public string? Kategori { get; set; }

        [Column("deskripsi", TypeName = "text")]
        public string? Deskripsi { get; set; }

        // Navigasi: Satu barang bisa ada di banyak pemesanan
        public ICollection<Pemesanan>? ListPemesanan { get; set; }
    }
}
