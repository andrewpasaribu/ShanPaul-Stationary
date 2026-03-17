using Microsoft.AspNetCore.Mvc;
using TokoAlatTulisAPI.DTOs;
using TokoAlatTulisAPI.Services;

namespace TokoAlatTulisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarangController : ControllerBase
    {
        private readonly IBarangService _barangService;

        public BarangController(IBarangService barangService)
        {
            _barangService = barangService;
        }

        // GET: api/barang
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BarangDto>>> GetAll()
        {
            var data = await _barangService.GetAllBarangAsync();
            return Ok(data);
        }

        // GET: api/barang/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BarangDto>> GetById(int id)
        {
            var barang = await _barangService.GetBarangByIdAsync(id);
            if (barang == null)
            {
                return NotFound(new { message = "Barang tidak ditemukan" });
            }
            return Ok(barang);
        }

        // POST: api/barang
        [HttpPost]
        public async Task<ActionResult<BarangDto>> Create([FromBody] CreateBarangDto createDto)
        {
            var createdBarang = await _barangService.CreateBarangAsync(createDto);

            // Mengembalikan status 201 Created beserta Header Location
            return CreatedAtAction(nameof(GetById), new { id = createdBarang.IdBarang }, createdBarang);
        }

        // PUT: api/barang/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateBarangDto updateDto)
        {
            var isUpdated = await _barangService.UpdateBarangAsync(id, updateDto);

            if (!isUpdated)
            {
                return NotFound(new { message = "Gagal update, barang tidak ditemukan" });
            }

            return NoContent(); // Status 204 (Sukses tapi tidak ada konten balikan)
        }

        // DELETE: api/barang/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _barangService.DeleteBarangAsync(id);

            if (!isDeleted)
            {
                return NotFound(new { message = "Gagal hapus, barang tidak ditemukan" });
            }

            return NoContent();
        }

    }
}
