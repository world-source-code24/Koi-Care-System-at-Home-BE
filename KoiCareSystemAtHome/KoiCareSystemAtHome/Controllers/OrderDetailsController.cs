using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Controllers
{
    public class OrderDetailsController : ControllerBase
    {
        private readonly KoiCareSystemDbContext _context;
        private readonly INormalFunctionsRepository _normalFunctions;

        public OrderDetailsController(KoiCareSystemDbContext context, INormalFunctionsRepository normalFunctions)
        {
            _context = context;
            _normalFunctions = normalFunctions;
        }

        [HttpGet("Get-All-Order-Details")]
        public async Task<ActionResult<IEnumerable<List<OrderDetailsDTO>>>> GetAllOrderDetails(int orderId)
        {
            var orderDetailList = await _context.OrderDetailsTbls
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
            if (orderDetailList == null) return NotFound();
            return Ok(orderDetailList);
        }

        [HttpPut("Add-Order-Details (In Progress)")]
        public async Task<IActionResult> CreateOrderDetails(int orderID, List<CartDTO> cartDTOs)
        {
            var existingOrder = await _context.OrdersTbls.FindAsync(orderID);
            if (existingOrder == null) return NotFound(new { message = "Can't find orderID" });
            int? userId = existingOrder.AccId;
            if (existingOrder == null)
            {
                return NotFound(new { message = "Order not found", status = false });
            }
            // Scare will lose performance
            // To check if there any bug in progress
            // Will delete if sure
            foreach (var cart in cartDTOs)
            {
                if (cart.AccId != userId) return BadRequest("User cart didn't match user order id");
            }
            List<OrderDetailsTbl> listOrder = new List<OrderDetailsTbl>();
            foreach (var cart in cartDTOs)
            {
                OrderDetailsTbl orderDetailsTbl = new OrderDetailsTbl
                {
                    OrderId = orderID,
                    ProductId = cart.ProductId,
                    Quantity = cart.Quantity,
                    TotalPrice = _normalFunctions.TotalMoneyOfItem(cart.ProductId, cart.Quantity)
                };
                Console.WriteLine(orderDetailsTbl);
                listOrder.Add(orderDetailsTbl);
            }
            _context.OrderDetailsTbls.AddRange(listOrder);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Success", status = true });
        }
    }
}
