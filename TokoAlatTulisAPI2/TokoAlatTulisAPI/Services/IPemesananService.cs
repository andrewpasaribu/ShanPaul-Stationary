using TokoAlatTulisAPI.DTOs;

namespace TokoAlatTulisAPI.Services
{
    public interface IPemesananService
    {
        Task<IEnumerable<PemesananDto>> GetAllPemesananAsync();
        Task<PemesananDto?> GetPemesananByIdAsync(int id);

        // Return string untuk pesan error (null jika sukses) atau gunakan Tuple/Result pattern
        Task<(PemesananDto? result, string? errorMessage)> CreatePemesananAsync(CreatePemesananDto createDto);

        Task<bool> DeletePemesananAsync(int id);
    }
}
