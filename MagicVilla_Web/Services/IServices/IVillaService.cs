using MagicVilla_Web.Models.DTO;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllSync<T>();
        Task<T> GetSync<T>(int id);
        Task<T> CreateSync<T>(VillaCreateDTO dto);
        Task<T> UpdateSync<T>(VillaCreateDTO dto);
        Task<T> DeleteSync<T>(int id);
    }
}
