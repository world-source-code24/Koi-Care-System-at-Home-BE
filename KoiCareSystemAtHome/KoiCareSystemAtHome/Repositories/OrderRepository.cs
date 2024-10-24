using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace KoiCareSystemAtHome.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly KoicareathomeContext _context;
        private readonly INormalFunctionsRepository _normalFunctionsRepository;
        private readonly ICartRepository _cartRepository;

        public OrderRepository(ICartRepository cartRepository, KoicareathomeContext context, INormalFunctionsRepository normalFunctionsRepository)
        {
            _context = context;
            _normalFunctionsRepository = normalFunctionsRepository;
            _cartRepository = cartRepository;
        }


        //public List<OrderDTO> GetListOrder(int id)
        //{
        //    var orderDb = _context.OrdersTbls.Where(o => o.Equals(id)).ToList();

        //    // Check dk
        //    if (orderDb.Count > 0) { return null; }

        //    foreach (var orderStore in orderDb)
        //    {
        //        OrderDTO order = new OrderDTO
        //        {
        //            AccId = id,
        //            Date = DateOnly.FromDateTime(DateTime.Now),
        //            StatusOrder = AllEnum.OrderStatus.Pending.ToString(),
        //            StatusPayment = AllEnum.StatusPayment.Unpaid.ToString(),
        //            TotalAmount = totalAmount

        //        };
        //    }
        //}

        public List<int> GetOrderId(int id)
        {
            var orderTbl = _context.OrdersTbls.Select(o => o.OrderId).Where(o => o.Equals(id)).ToList();
            if (orderTbl == null)
            {
                return null;
            }
            else
            {
                return orderTbl;
            }
        }

        public async Task<List<OrdersTbl>> GetOrdersByAccId(int accId)
        {
            return await _context.OrdersTbls.Where(o => o.AccId == accId).ToListAsync();
        }

        public async Task<(bool, int)> CreateOrder(int id)
        {
            try
            {
                var totalCart = await _cartRepository.GetUserCarts(id);
                var totalAmount = await _normalFunctionsRepository.TotalMoneyOfCarts(totalCart);

                var order = new OrdersTbl
                {
                    AccId = id,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    StatusOrder = AllEnum.OrderStatus.Pending.ToString(),
                    StatusPayment = AllEnum.StatusPayment.Unpaid.ToString(),
                    TotalAmount = totalAmount
                };

                _context.OrdersTbls.Add(order);
                await _context.SaveChangesAsync();
                Console.WriteLine(order.OrderId);
                return (true, order.OrderId);
            }
            catch (Exception ex)
            {
                // Log the exception to help track down issues
                return (false, -1);
            }
        }
    }
}
