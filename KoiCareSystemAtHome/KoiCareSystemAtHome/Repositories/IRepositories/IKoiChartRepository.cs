using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IKoiChartRepository : IGenericRepository<KoiGrowthChartsTbl>
    {
        Task<List<KoiGrowthChartsTbl>> GetKoiGrowthCharts(int koiId);
        //Task UpdateKoiGrowthCharts(int koiId, KoiDTO koiDTO);
    }
}
