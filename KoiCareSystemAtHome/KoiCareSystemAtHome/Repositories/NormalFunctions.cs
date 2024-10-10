using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;

namespace KoiCareSystemAtHome.Repositories
{
    public class NormalFunctions : INormalFunctionsRepository
    {
        private readonly ICartDetailsRepository _repository;

        public NormalFunctions(ICartDetailsRepository repository)
        {
            _repository = repository;
        }

        public decimal TotalMoneyOfItem(int quantity, int price)
        {
            return quantity * price;
        }

        public async Task<decimal> TotalMoneyOfCarts(List<CartDTO> cartList)
        {
            decimal totalMoney = 0;

            foreach (var cart in cartList)
            {
                //Lay product price
                var product = await _repository.GetProductInfo(cart.ProductId);
                decimal price = product.Price;
                
                totalMoney += cart.Quantity * price;
            }

            return totalMoney;
        }

    }
}
