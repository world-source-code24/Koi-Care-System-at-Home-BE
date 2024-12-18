﻿using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
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
        /*
         1 - unpaid va processing
        2 - unpaid va shipping
        3 - completed va paid
        4 - ship completed paid
        5 - cancelled 
         */
        public async Task<bool> SetOrderStatus(int orderId, int status)
        {
            var order = await GetOrder(orderId);
            if (order == null) return false;
            if (status < 1 || status > 5) return false;
            if (status == 1)
            {
                order.StatusOrder = AllEnum.OrderStatus.Processing.ToString();

            }
            if (status == 2)
            {
                order.StatusOrder = AllEnum.OrderStatus.Shiping.ToString();

            }
            if (status == 3)
            {
                order.StatusOrder = AllEnum.OrderStatus.ShipCompleted.ToString();
            }
            if (status == 4)
            {
                order.StatusOrder = AllEnum.OrderStatus.Completed.ToString();

            }

            if (status == 5)
            {
                order.StatusOrder = AllEnum.OrderStatus.Cancelled.ToString();
                order.StatusPayment = AllEnum.StatusPayment.Cancelled.ToString();
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OrdersTbl> GetOrder(int orderId)
        {
            var order = await _context.OrdersTbls.Where(o => o.OrderId == orderId).FirstOrDefaultAsync();
            return order;
        }
    }
}
