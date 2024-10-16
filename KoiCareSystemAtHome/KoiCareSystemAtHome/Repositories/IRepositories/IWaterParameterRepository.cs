using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IWaterParameterRepository : IGenericRepository<WaterParametersTbl>
    {
        Task<WaterParametersTbl> GetByPondIdAsync(int id);
        Task<List<WaterParametersTbl>> GetAllByPondIdAsync(int pondId);
        Task<WaterParametersTbl> UpdateParameter(int pondId, WaterParametersTbl updateParam, WaterParameterDTO param);
    }
}