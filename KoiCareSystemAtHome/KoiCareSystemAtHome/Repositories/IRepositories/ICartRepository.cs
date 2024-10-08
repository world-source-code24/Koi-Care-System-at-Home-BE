using KoiCareSystemAtHome.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface ICartRepository
    {
        Task<(bool IsSuccess, string Message)> CheckStockAndProcessOrder(int productId, int requestedQuantity);

        Task<List<CartDTO>> GetUserCarts(int userID);
    }
}
