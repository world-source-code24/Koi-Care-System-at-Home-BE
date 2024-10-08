using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface ICartDetailsRepository
    {
        //Get product
        Task<CartDetailsDTO> GetProductInfo(int productId);

     
    }
}
