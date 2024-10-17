using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Controllers
{
    public class PaymentController : ControllerBase
    {
        private readonly OrderController _orderController;
        private readonly OrderDetailsController _orderDetailsController;
        private readonly ICartRepository _cartRepository;

        public PaymentController( OrderController orderController, OrderDetailsController orderDetailsController, ICartRepository cartRepository)
        {
            _orderController = orderController;
            _orderDetailsController = orderDetailsController;
            _cartRepository = cartRepository;
        }

        [HttpPut ("Add-Order,OrderDetails-Delete-Cart(In progress)")]
        public async Task<IActionResult> TotalWhatHappenWhenPayment(int userID)
        {
            try
            {
                    // Lay cart list
                var cartList = await _cartRepository.GetUserCarts (userID);
                if (cartList == null) return NotFound("Not found cart with this user");
                // Lay order list
                _orderDetailsController.CreateOrderDetails(userID, cartList);
                _cartRepository.DeleteAllCart(userID);
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
