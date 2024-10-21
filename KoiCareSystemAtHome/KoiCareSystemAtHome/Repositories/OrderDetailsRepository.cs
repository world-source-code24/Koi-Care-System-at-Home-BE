using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Repositories
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly KoiCareSystemDbContext _context;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly INormalFunctionsRepository _normalFunctionsRepository;

        public OrderDetailsRepository(KoiCareSystemDbContext context, ICartRepository cartRepository, IOrderRepository orderRepository, INormalFunctionsRepository normalFunctionsRepository)
        {
            _context = context;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _normalFunctionsRepository = normalFunctionsRepository;
        }

        public async Task<bool> CreateOrderDetails(int orderID, List<CartDTO> cartDTOs)
        {
            try
            {
                List<OrderDetailsTbl> listOrder = new List<OrderDetailsTbl>();
                foreach (var cart in cartDTOs)
                {
                    OrderDetailsTbl orderDetailsTbl = new OrderDetailsTbl
                    {
                        OrderId = orderID,
                        ProductId = cart.ProductId,
                        Quantity = cart.Quantity,
                        TotalPrice = _normalFunctionsRepository.TotalMoneyOfItem(cart.ProductId, cart.Quantity)
                    };
                    listOrder.Add(orderDetailsTbl);
                }

                _context.OrderDetailsTbls.AddRange(listOrder);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {               
                return false;
            }
        }

    }
}
