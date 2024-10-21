using KoiCareSystemAtHome.Entities;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IShopRepository : IGenericRepository<ShopsTbl>
    {
        Task<ShopsTbl> GetByShopIdAsync(int shopId);
        Task<List<ShopsTbl>> GetAllShopsAsync();
        Task<int> GetTotalShops();
    }
}
