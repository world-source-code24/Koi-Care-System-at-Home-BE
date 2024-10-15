using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Controllers
{
    public class PaymentController : ControllerBase
    {
        private readonly CartController _cartController;
        private readonly OrderController _orderController;
        private readonly OrderDetailsController _orderDetailsController;
        private readonly ICartRepository _cartRepository;

        public PaymentController(CartController cartController, OrderController orderController, OrderDetailsController orderDetailsController, ICartRepository cartRepository)
        {
            _cartController = cartController;
            _orderController = orderController;
            _orderDetailsController = orderDetailsController;
            _cartRepository = cartRepository;
        }

        [HttpPut ("Add-Order,OrderDetails-Delete-Cart.(Ko the goi tat duoc)")]
        public async Task<IActionResult> TotalWhatHappenWhenPayment(int userID)
        {
            try
            {
                    // Lay cart ra chuyen qua order, orderdetails, orderDetails se lay tu cart, 
                var cartList = await _cartRepository.GetUserCarts (userID);
                if (cartList == null) return NotFound("Not found cart with this user");
                await _orderController.CreateOrderAndMoney(userID);
                await _orderDetailsController.CreateOrderDetails(userID, cartList);
                await _cartController.DeleteAllUserCarts(userID);
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
