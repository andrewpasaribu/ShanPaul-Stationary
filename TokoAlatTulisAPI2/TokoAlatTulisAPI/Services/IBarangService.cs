using TokoAlatTulisAPI.DTOs;

namespace TokoAlatTulisAPI.Services
{
    public interface IBarangService
    {
        Task<IEnumerable<BarangDto>> GetAllBarangAsync();
        Task<BarangDto?> GetBarangByIdAsync(int id);
        Task<BarangDto> CreateBarangAsync(CreateBarangDto createDto);
        Task<bool> UpdateBarangAsync(int id, CreateBarangDto updateDto);
        Task<bool> DeleteBarangAsync(int id);
    }

}
