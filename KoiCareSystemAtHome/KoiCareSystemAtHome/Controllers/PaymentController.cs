using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Controllers
{
    public class PaymentController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly ICartRepository _cartRepository;
        private readonly KoiCareSystemDbContext _context; 

        public PaymentController(IOrderRepository orderRepository, IOrderDetailsRepository orderDetailsRepository, ICartRepository cartRepository, KoiCareSystemDbContext context)
        {
            _orderRepository = orderRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _cartRepository = cartRepository;
            _context = context; 
        }

        [HttpPut ("Payment(In-Progress)")]
        public async Task<IActionResult> TotalWhatHappenWhenPayment(int userID)
        {
            using (var transaction = _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Lay cart list de add orderDetails
                    var cartList = await _cartRepository.GetUserCarts(userID);
                    if (cartList == null) return NotFound("Not found cart with this user");
                    //Add Order
                    var (bOrder, iOrderId) = await _orderRepository.CreateOrder(userID);
                    if (!bOrder) return BadRequest("Error at Create Order");
                    // Add vo OrderDetails
                    Console.WriteLine(iOrderId);
                    bool bOrderDetails = await _orderDetailsRepository.CreateOrderDetails(iOrderId, cartList);
                    // Delete tat ca cart tu user do
                    if (!bOrderDetails) return BadRequest("Error at Create Order Details");
                    if (!_cartRepository.DeleteAllCart(userID)) return BadRequest("Error at delete cart");
                    await _context.SaveChangesAsync();// Fix
                    return Ok(new { message = "Success" });
                }
                catch (Exception ex)
                {
                    // Return an error response if an exception occurs
                    return BadRequest(new { message = "An error occurred", error = ex.Message });
                }
            }
        }


    }
}
