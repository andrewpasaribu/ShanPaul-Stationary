namespace TokoAlatTulisAPI.DTOs
{
    public class PemesananDto
    {
        public int IdPemesanan { get; set; }
        public int? IdBarang { get; set; }

        // Opsional: Menampilkan nama barang di response agar lebih informatif
        public string? NamaBarang { get; set; }

        public string NamaPembeli { get; set; } = string.Empty;
        public int Jumlah { get; set; }
        public decimal? TotalBayar { get; set; }
        public DateTime TanggalPemesanan { get; set; }
    }
}
