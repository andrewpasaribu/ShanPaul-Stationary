const API_BASE_URL = "http://localhost:5252/api";

document.addEventListener("DOMContentLoaded", () => {
  loadBarang();
  loadPemesanan();
});

async function loadBarang() {
  try {
    const response = await fetch(`${API_BASE_URL}/barang`);
    const data = await response.json();
    const select = document.getElementById("selectBarang");
    select.innerHTML = '<option value="">-- Pilih Barang --</option>';
    data.forEach((barang) => {
      const option = document.createElement("option");
      option.value = barang.idBarang;
      option.textContent = `${barang.namaBarang} (Stok: ${barang.stok}) - Rp ${barang.harga}`;
      select.appendChild(option);
    });
  } catch (error) {
    console.error("Gagal memuat barang:", error);
  }
}

async function loadPemesanan() {
  try {
    const response = await fetch(`${API_BASE_URL}/pemesanan`);
    const data = await response.json();
    const tableBody = document.getElementById("tableBody");
    tableBody.innerHTML = "";
    data.forEach((pesanan) => {
      const row = document.createElement("tr");
      const total = new Intl.NumberFormat("id-ID", {
        style: "currency",
        currency: "IDR",
      }).format(pesanan.totalBayar);

      row.innerHTML = `
                <td>${pesanan.idPemesanan}</td>
                <td>${pesanan.namaBarang}</td>
                <td>${pesanan.namaPembeli}</td>
                <td>${pesanan.jumlah}</td>
                <td>${total}</td>
                <td>${new Date(pesanan.tanggalPemesanan).toLocaleDateString(
                  "id-ID",
                  {
                    day: "numeric",
                    month: "short",
                    year: "numeric",
                    hour: "2-digit",
                    minute: "2-digit",
                  },
                )}</td>
                <td><button class="btn-danger" onclick="hapusPesanan(${pesanan.idPemesanan})">Hapus</button></td>`;
      tableBody.appendChild(row);
    });
  } catch (error) {
    console.error("Gagal memuat pesanan:", error);
  }
}

document.getElementById("orderForm").addEventListener("submit", async (e) => {
  e.preventDefault();
  const idBarang = document.getElementById("selectBarang").value;
  const namaPembeli = document.getElementById("inputNama").value;
  const jumlah = document.getElementById("inputJumlah").value;

  if (!idBarang) {
    alert("Harap pilih barang!");
    return;
  }

  const payload = {
    idBarang: parseInt(idBarang),
    namaPembeli: namaPembeli,
    jumlah: parseInt(jumlah),
  };

  try {
    const response = await fetch(`${API_BASE_URL}/pemesanan`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload),
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || "Gagal membuat pesanan");
    }

    alert("Pesanan berhasil dibuat!");
    document.getElementById("orderForm").reset();
    loadPemesanan();
    loadBarang();
  } catch (error) {
    alert(error.message);
  }
});

async function hapusPesanan(id) {
  if (
    !confirm(
      "Apakah Anda yakin ingin menghapus pesanan ini? Stok akan dikembalikan.",
    )
  )
    return;
  try {
    const response = await fetch(`${API_BASE_URL}/pemesanan/${id}`, {
      method: "DELETE",
    });
    if (response.ok) {
      alert("Pesanan berhasil dihapus.");
      loadPemesanan();
      loadBarang();
    } else {
      alert("Gagal menghapus pesanan.");
    }
  } catch (error) {
    console.error("Error:", error);
  }
}
