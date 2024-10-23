using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using System.Threading.Tasks;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        List<int> GetOrderId(int id);

        Task<(bool,int)> CreateOrder(int id);
        //List<OrderDTO> GetListOrder (int id);

        Task<List<OrdersTbl>> GetOrdersByAccId(int accId);
    }
}
