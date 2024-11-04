using KoiCareSystemAtHome.Entities;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IProductRepository : IGenericRepository<ProductsTbl>
    {
        Task<ProductsTbl> GetByProductIdAsync(int id);
        Task<List<ProductsTbl>> GetAllByShopIdAsync(int shopId);
        Task<List<ProductsTbl>> GetAllProductsAsync();
        Task<List<ProductsTbl>> SearchByNameAsync(string name);
        Task<int> GetTotalProduct();
    }
}
