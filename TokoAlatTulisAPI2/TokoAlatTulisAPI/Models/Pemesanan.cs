using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokoAlatTulisAPI.Models
{
    [Table("pemesanan")]
    public class Pemesanan
    {
        [Key]
        [Column("id_pemesanan")]
        public int IdPemesanan { get; set; }

        // Nullable (int?) karena di SQL didefinisikan ON DELETE SET NULL
        [Column("id_barang")]
        public int? IdBarang { get; set; }

        [Required]
        [Column("nama_pembeli")]
        [StringLength(100)]
        public string NamaPembeli { get; set; } = string.Empty;

        [Required]
        [Column("jumlah")]
        public int Jumlah { get; set; }

        [Column("total_bayar", TypeName = "decimal(10, 2)")]
        public decimal? TotalBayar { get; set; }

        [Column("tanggal_pemesanan")]
        public DateTime TanggalPemesanan { get; set; } = DateTime.Now;

        // Navigasi: Relasi ke tabel Barang
        [ForeignKey(nameof(IdBarang))]
        public Barang? Barang { get; set; }
    }
}
