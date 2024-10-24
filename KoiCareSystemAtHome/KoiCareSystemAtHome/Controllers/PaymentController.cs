using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Controllers
{
    public class PaymentController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly ICartRepository _cartRepository;
        private readonly KoicareathomeContext _context;
        private readonly IVnPayService _vnPayService;

        public PaymentController(IOrderRepository orderRepository, IOrderDetailsRepository orderDetailsRepository, ICartRepository cartRepository, KoicareathomeContext context, IVnPayService vnPayService)
        {
            _orderRepository = orderRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _cartRepository = cartRepository;
            _context = context;
            _vnPayService = vnPayService;
        }

        [HttpPut("/api/Payment")]
        public async Task<IActionResult> Payment(int userID)
        {
            // Start a database transaction
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Get cart list for the user
                var cartList = await _cartRepository.GetUserCarts(userID);
                if (cartList == null)
                    return NotFound(new { message = "Cart not found for this user" });

                // Create an order
                var (bOrder, iOrderId) = await _orderRepository.CreateOrder(userID);
                if (!bOrder)
                    return BadRequest(new { message = "Failed to create order" });

                // Create order details based on the cart list
                bool bOrderDetails = await _orderDetailsRepository.CreateOrderDetails(iOrderId, cartList);
                if (!bOrderDetails)
                    return BadRequest(new { message = "Failed to create order details" });

                // Delete all cart items for this user
                bool deleteCartSuccess = await _cartRepository.DeleteAllCart(userID);
                if (!deleteCartSuccess)
                    return BadRequest(new { message = "Failed to delete user cart" });

                // Save changes to the database and commit the transaction
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Return success response
                return Ok(new { message = "Payment processed successfully" });
            }
            catch (Exception ex)
            {
                // Rollback transaction in case of exception
                await transaction.RollbackAsync();
                return BadRequest(new { message = "An error occurred during payment processing", error = ex.Message });
            }
        }

        [HttpGet("/api/Tranfer-Order-To-VnPayModel")]
        public async Task<PaymentInformationModel> OrderToVnPayModel (int orderID)
        {
            var order = await _context.OrdersTbls.Where(o => o.OrderId == orderID).FirstOrDefaultAsync();
            PaymentInformationModel model = new PaymentInformationModel
            {
                Amount = (float)order.TotalAmount.Value,
                Name = orderID.ToString(),
                OrderDescription = "KoiItem",   
                OrderType = "Item"

            };
            return model;
        }

        [HttpPost("/api/VnCreatePayment")]
        public IActionResult CreatePayment(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
            Console.WriteLine(url);
            return Redirect(url);
        }

        [HttpPost("/api/VnCreatePaymentString")]
        public ActionResult<string> CreatePaymentTest(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
            return Ok(url);
        }

        [HttpGet("/api/VnPaymentCallback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            return Ok(response);
        }

    }
}
