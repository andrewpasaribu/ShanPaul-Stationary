public class BarangDto
{
    public int IdBarang { get; set; }
    public string NamaBarang { get; set; } = string.Empty;

    public decimal Harga { get; set; }
    public int Stok { get; set; }
    public string Kategori { get; set; }
    public string Deskripsi { get; set; }
}

public class CreateBarangDto
{
    public string NamaBarang { get; set; } = string.Empty;
    public decimal Harga { get; set; }
    public int Stok { get; set; }
    public string Kategori { get; set; }
    public string Deskripsi { get; set; }
}