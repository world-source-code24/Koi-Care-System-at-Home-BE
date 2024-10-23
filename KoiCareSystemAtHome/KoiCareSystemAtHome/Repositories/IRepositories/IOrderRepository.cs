using KoiCareSystemAtHome.Models;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        List<int> GetOrderId(int id);

        Task<(bool,int)> CreateOrder(int id);
        //List<OrderDTO> GetListOrder (int id);
    }
}
