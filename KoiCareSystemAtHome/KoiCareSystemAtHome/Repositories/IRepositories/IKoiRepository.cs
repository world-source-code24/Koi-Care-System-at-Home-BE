using KoiCareSystemAtHome.Entities;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IKoiRepository : IGenericRepository<KoisTbl>
    {
        //Get Koi Detail
        Task<KoisTbl> GetKoiByKoiIdAsync(int koiId);
        //Get All Koi In The Pond of User
        //We use userId and pondId
        Task<List<KoisTbl>> GetKoiByUserIdAsync(int userId);
        //Get All Koi by PondId
        Task<List<KoisTbl>> GetKoiByPondIdAsync(int pondId);
        //Save Koi to Chart
        Task SaveKoiToChartAsync(int koiId);
    }
}
