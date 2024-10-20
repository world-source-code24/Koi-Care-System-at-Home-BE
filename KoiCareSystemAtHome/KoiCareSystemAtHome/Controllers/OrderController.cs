using KoiCareSystemAtHome.Entities;
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

        public OrderController(KoiCareSystemDbContext context, INormalFunctionsRepository normalFunctionsRepository, ICartRepository cartRepository, AllEnum aenum)
        {
            _context = context;
            _normalFunctionsRepository = normalFunctionsRepository;
            _cartRepository = cartRepository;
            _aenum = aenum;
        }

        [HttpGet("Get-Order")]
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

        [HttpPost("Create-Order-Without-Calculate-Money")]
        public async Task<IActionResult> CreateOrder(int accID)
        {
            var order = new OrdersTbl
            {
                AccId = accID,
                Date = DateOnly.FromDateTime(DateTime.Now),
                StatusOrder = "Pending",
                StatusPayment = "Unpaid",
                TotalAmount = 0
            };
            _context.OrdersTbls.Add(order);
            await _context.SaveChangesAsync();
            return Ok(new { status = true, message = "Add order" });
        }

        [HttpPost("Create-Order-And-Calculate-Money")]
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

        [HttpPut("Update-Status-Payment")]
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

        [HttpPut("Update-Status-Payment-Latest")]
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

        [HttpGet("Get-All-Order-By-Category")]
        public async Task<IActionResult> GetOrderByCategory(string category)
        {
            var orders = await (from order in _context.OrdersTbls
                                join orderDetail in _context.OrderDetailsTbls
                                on order.OrderId equals orderDetail.OrderId
                                join product in _context.ProductsTbls
                                on orderDetail.ProductId equals product.ProductId
                                where product.Category == category
                                select new OrderDTO
                                {
                                    OrderId = order.OrderId,
                                    Date = order.Date,
                                    StatusOrder = order.StatusOrder,
                                    TotalAmount = order.TotalAmount,
                                }).ToListAsync();
            if (!orders.Any())
                return Ok(new { message = "No orders found for the specified category." });
            return Ok(orders);
        }
    }
}
