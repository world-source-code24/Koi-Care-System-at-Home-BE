using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        List<int> GetOrderId(int id);

        Task<(bool,int)> CreateOrder(int id);
        //List<OrderDTO> GetListOrder (int id);

        Task<List<OrdersTbl>> GetOrdersByAccId(int accId);

        Task<bool> SetOrderStatus(int orderId, int status);

        Task<OrdersTbl> GetOrder(int orderId);
    }
}
