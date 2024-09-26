using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;

namespace KoiCareSystemAtHome.Repositories
{
    public interface IWaterParameterRepository : IGenericRepository<WaterParametersTbl>
    {
        Task<WaterParametersTbl> GetByPondIdAsync(int id);
        Task<List<WaterParametersTbl>> GetAllByPondIdAsync(int pondId);
    }
}
