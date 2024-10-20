using KoiCareSystemAtHome.Models;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IOrderDetailsRepository
    {
        Task<bool> CreateOrderDetails(int orderID, List<CartDTO> cartDTOs);
    }
}
