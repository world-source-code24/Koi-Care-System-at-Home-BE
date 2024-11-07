using KoiCareSystemAtHome.Entities;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IShopRepository : IGenericRepository<ShopsTbl>
    {
        Task<ShopsTbl> GetByShopIdAsync(int shopId);
        Task<List<ShopsTbl>> GetAllShopsAsync();
        Task <ShopsTbl> GetShopByCode(string code);
        Task<int> GetTotalShops();
    }
}
