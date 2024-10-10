using KoiCareSystemAtHome.Models;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface INormalFunctionsRepository
    {
        public decimal TotalMoneyOfItem(int quantity, int price);
        Task<decimal> TotalMoneyOfCarts(List<CartDTO> cartList);
    }
}
