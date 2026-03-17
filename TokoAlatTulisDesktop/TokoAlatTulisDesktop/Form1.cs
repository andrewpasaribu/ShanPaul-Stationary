using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TokoAlatTulisDesktop
{
    public partial class Form1 : Form
    {
        // Setup HttpClient dengan Base URL API
        private readonly HttpClient _client = new HttpClient
        {
            // GANTI PORT DI SINI SESUAI BACKEND ANDA
            BaseAddress = new Uri("http://localhost:5252/api/")
        };

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadData();
        }

        // --- METHOD BANTUAN ---

        private async Task LoadData()
        {
            try
            {
                var barangList = await _client.GetFromJsonAsync<List<BarangDto>>("barang");
                dgvData.DataSource = barangList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message);
            }
        }

        private void ClearForm()
        {
            lblId.Text = "0";
            txtNama.Clear();
            txtHarga.Clear();
            txtStok.Clear();
            txtKategori.Clear();
            txtDeskripsi.Clear();

            // Fokus kursor ke nama
            txtNama.Focus();
            dgvData.ClearSelection();
        }

        // --- EVENT HANDLERS ---

        // 1. TAMBAH (POST)
        private async void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNama.Text)) return;

            var newBarang = new CreateBarangDto
            {
                NamaBarang = txtNama.Text,
                Harga = decimal.TryParse(txtHarga.Text, out var h) ? h : 0,
                Stok = int.TryParse(txtStok.Text, out var s) ? s : 0,
                Kategori = txtKategori.Text,
                Deskripsi = txtDeskripsi.Text
            };

            try
            {
                var response = await _client.PostAsJsonAsync("barang", newBarang);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Barang berhasil disimpan!");
                    ClearForm();
                    await LoadData();
                }
                else
                {
                    MessageBox.Show("Gagal menyimpan.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // 2. EDIT (PUT)
        private async void btnEdit_Click(object sender, EventArgs e)
        {
            int id = int.Parse(lblId.Text);
            if (id == 0)
            {
                MessageBox.Show("Pilih barang dari tabel terlebih dahulu.");
                return;
            }

            var updateBarang = new CreateBarangDto
            {
                NamaBarang = txtNama.Text,
                Harga = decimal.TryParse(txtHarga.Text, out var h) ? h : 0,
                Stok = int.TryParse(txtStok.Text, out var s) ? s : 0,
                Kategori = txtKategori.Text,
                Deskripsi = txtDeskripsi.Text
            };

            try
            {
                var response = await _client.PutAsJsonAsync($"barang/{id}", updateBarang);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Barang berhasil diupdate!");
                    ClearForm();
                    await LoadData();
                }
                else
                {
                    MessageBox.Show("Gagal update.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // 3. HAPUS (DELETE)
        private async void btnHapus_Click(object sender, EventArgs e)
        {
            int id = int.Parse(lblId.Text);
            if (id == 0)
            {
                MessageBox.Show("Pilih barang dari tabel terlebih dahulu.");
                return;
            }

            var confirm = MessageBox.Show("Yakin ingin menghapus barang ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.No) return;

            try
            {
                var response = await _client.DeleteAsync($"barang/{id}");
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Barang berhasil dihapus!");
                    ClearForm();
                    await LoadData();
                }
                else
                {
                    MessageBox.Show("Gagal hapus.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // 4. RESET FORM
        private async void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            await LoadData();
        }

        // 5. PILIH BARANG DARI TABEL
        private void dgvData_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                var row = dgvData.SelectedRows[0];
                var item = (BarangDto)row.DataBoundItem;

                if (item != null)
                {
                    lblId.Text = item.IdBarang.ToString();
                    txtNama.Text = item.NamaBarang;
                    txtHarga.Text = item.Harga.ToString();
                    txtStok.Text = item.Stok.ToString();
                    txtKategori.Text = item.Kategori;
                    txtDeskripsi.Text = item.Deskripsi;
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
