using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly KoiCareSystemDbContext _context;

        [HttpGet("Get-Order")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrder (int id)
        {
            var order = await _context.OrdersTbls.FindAsync (id);
            if (order == null)
            {
                return NotFound("Not found order");
            }
            else
            {
                return Ok(new {message = "success", status = true, order });
            }
        }

        [HttpPut("Create-Order")]
        public async Task<IActionResult> CreateOrder (int accID)
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
            return Ok(new {status = true, message = "Add order"});
        }
    }
}
