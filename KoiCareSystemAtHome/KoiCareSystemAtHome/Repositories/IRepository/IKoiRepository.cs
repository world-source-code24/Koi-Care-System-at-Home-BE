using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Repositories.IRepositories;

namespace KoiCareSystemAtHome.Repositories.IRepository
{
    public interface IKoiRepository : IGenericRepository<KoisTbl>
    {
        //Get Koi Detail
        Task<KoisTbl> GetKoiByKoiId(int koiId);
        //Get All Koi In The Pond of User
        //We use userId and pondId
        Task<List<KoisTbl>> GetKoiByUserIdAndPondId(int userId, int pondId);
        //Get All Koi by PondId
        Task<List<KoisTbl>> GetKoiByPondId(int pondId);
        //Save Koi to Chart
        public void SaveKoiToChart(int koiId);
    }
}
