using Microsoft.AspNetCore.Mvc;
using TokoAlatTulisAPI.DTOs;
using TokoAlatTulisAPI.Services;

namespace TokoAlatTulisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PemesananController : ControllerBase
    {
        private readonly IPemesananService _pemesananService;

        public PemesananController(IPemesananService pemesananService)
        {
            _pemesananService = pemesananService;
        }

        // GET: api/pemesanan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PemesananDto>>> GetAll()
        {
            var data = await _pemesananService.GetAllPemesananAsync();
            return Ok(data);
        }

        // GET: api/pemesanan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PemesananDto>> GetById(int id)
        {
            var pemesanan = await _pemesananService.GetPemesananByIdAsync(id);
            if (pemesanan == null)
            {
                return NotFound(new { message = "Data pemesanan tidak ditemukan" });
            }
            return Ok(pemesanan);
        }

        // POST: api/pemesanan
        [HttpPost]
        public async Task<ActionResult<PemesananDto>> Create([FromBody] CreatePemesananDto createDto)
        {
            // Memanggil service yang mengembalikan Tuple (hasil, pesan_error)
            var (result, errorMessage) = await _pemesananService.CreatePemesananAsync(createDto);

            // Jika ada pesan error (misal: Stok habis), kembalikan 400 Bad Request
            if (errorMessage != null)
            {
                return BadRequest(new { message = errorMessage });
            }

            // Jika sukses, kembalikan 201 Created
            return CreatedAtAction(nameof(GetById), new { id = result!.IdPemesanan }, result);
        }

        // DELETE: api/pemesanan/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _pemesananService.DeletePemesananAsync(id);

            if (!isDeleted)
            {
                return NotFound(new { message = "Pemesanan tidak ditemukan" });
            }

            return NoContent();
        }
    }
}
