﻿using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly KoiCareSystemDbContext _context;
        private readonly INormalFunctionsRepository _normalFunctionsRepository;
        private readonly ICartRepository _cartRepository;
        private readonly AllEnum _aenum;
        private readonly IOrderRepository _orderRepository;

        public OrderController(KoiCareSystemDbContext context, INormalFunctionsRepository normalFunctionsRepository, ICartRepository cartRepository, AllEnum aenum, IOrderRepository orderRepository)
        {
            _context = context;
            _normalFunctionsRepository = normalFunctionsRepository;
            _cartRepository = cartRepository;
            _aenum = aenum;
            _orderRepository = orderRepository;
        }

        [HttpGet("/api/Get-Order")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrder(int id)
        {
            var order = await _context.OrdersTbls.FindAsync(id);
            if (order == null)
            {
                return NotFound("Not found order");
            }
            else
            {
                return Ok(new { message = "success", status = true, order });
            }
        }

        //[HttpPost("Create-Order-Without-Calculate-Money")]
        //public async Task<IActionResult> CreateOrder(int accID)
        //{
        //    var order = new OrdersTbl
        //    {
        //        AccId = accID,
        //        Date = DateOnly.FromDateTime(DateTime.Now),
        //        StatusOrder = "Pending",
        //        StatusPayment = "Unpaid",
        //        TotalAmount = 0
        //    };
        //    _context.OrdersTbls.Add(order);
        //    await _context.SaveChangesAsync();
        //    return Ok(new { status = true, message = "Add order" });
        //}


        [HttpPost("/api/Create-Order-And-Calculate-Money")]
        public async Task<IActionResult> CreateOrderAndMoney(int accID)
        {
            try
            {
                var totalCart = await _cartRepository.GetUserCarts(accID);
                var totalAmount = await _normalFunctionsRepository.TotalMoneyOfCarts(totalCart);
                var order = new OrdersTbl
                {
                    AccId = accID,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    StatusOrder = AllEnum.OrderStatus.Pending.ToString(),
                    StatusPayment = AllEnum.StatusPayment.Unpaid.ToString(),
                    TotalAmount = totalAmount
                };
                _context.OrdersTbls.Add(order);
                await _context.SaveChangesAsync();
                return Ok(new { status = true, message = "Add order" });
            }
            catch (NullReferenceException n)
            {
                return BadRequest(n);
            }
        }

        [HttpPut("/api/Update-Status-Payment")]
        public async Task<IActionResult> UpdatePaidSuccess(int accID, int orderID)
        {
            var order = await _context.OrdersTbls
                .Where(order => order.AccId == accID && order.OrderId == orderID)
                .FirstOrDefaultAsync();
            if (order == null) return NotFound("Not found this order");
            order.StatusPayment = AllEnum.StatusPayment.Paid.ToString();
            await _context.SaveChangesAsync();
            return Ok(new { status = true, message = "Payment status updated to Paid." });
        }

        [HttpPut("/api/Update-Status-Payment-Latest")]
        public async Task<IActionResult> UpdatePaidSuccessLatest(int accID, int orderID)
        {
            var order = await _context.OrdersTbls
                .Where(order => order.AccId == accID)
                .OrderByDescending(order => orderID)
                .FirstOrDefaultAsync();
            if (order == null) return NotFound("Not found this order");
            order.StatusPayment = AllEnum.StatusPayment.Paid.ToString();
            await _context.SaveChangesAsync();
            return Ok(new { status = true, message = "Payment status updated to Paid." });
        }


    }
}
