using KoiCareSystemAtHome.Entities;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IKoiRepository : IGenericRepository<KoisTbl>
    {
        Task<KoisTbl> GetAllKoiByPondId(int pondId);
        Task<List<KoisTbl>> GetKoiByName(string koiName);
    }
}
