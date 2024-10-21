using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace KoiCareSystemAtHome.Controllers
{
    public class OrderDetailsController : ControllerBase
    {
        private readonly KoiCareSystemDbContext _context;
        private readonly INormalFunctionsRepository _normalFunctions;
        private readonly ICartRepository _cart;

        public OrderDetailsController(KoiCareSystemDbContext context, INormalFunctionsRepository normalFunctions, ICartRepository cart)
        {
            _context = context;
            _normalFunctions = normalFunctions;
            _cart = cart;
        }

        [HttpGet("/api/Get-All-Order-Details")]
        public async Task<ActionResult<IEnumerable<List<OrderDetailsDTO>>>> GetAllOrderDetails(int orderId)
        {
            var orderDetailList = await _context.OrderDetailsTbls
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
            if (orderDetailList == null) return NotFound();
            return Ok(orderDetailList);
        }

        [HttpPut("/api/Add-Order-Details(Read-Code-Note)")]
        public async Task<IActionResult> CreateOrderDetails(int orderID, List<CartDTO> cartDTOs)

        // Vi mot li do nao do tren swagger ko nhan list, nhung neu su dung ham
        // Lay cart van se nhan


        {
            // lAY ORDERID
            //var existingOrder = await _context.OrdersTbls.FindAsync(orderID);
            //if (existingOrder == null) return NotFound(new { message = "Can't find orderID" });       
            //var cartDTOs =await _cart.GetUserCarts(1);
            // lAY User Id tu OrderId do
            //int? userId = existingOrder.AccId;
            //Console.WriteLine("Order ID");
            //Console.WriteLine(existingOrder.OrderId);
            //Console.WriteLine("User ID");
            //Console.WriteLine(userId);
            // Scare will lose performance
            // To check if there any bug in progress
            // Will delete if sure
            // Check userID voi OrderId co match nhau ko
            //foreach (var cart in cartDTOs)
            //{
            //    if (cart.AccId != userId) return BadRequest("User cart didn't match user order id");
            //}
            //if (cartDTOs.Count ==0)
            //{
            //    return NotFound("The cart is empty");
            //}
            //Console.WriteLine($"Received {cartDTOs.Count} cart items.");
            //foreach (var cart in cartDTOs)
            //{
            //    Console.WriteLine($"AccId: {cart.AccId}, ProductId: {cart.ProductId}, Quantity: {cart.Quantity}");
            //}

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
            // Nhap sau de tranh bi loi trong qua trinh tranfer cart, order.
            foreach (var order in listOrder)
            {
                _context.OrderDetailsTbls.Add(order);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Success", status = true });
        }

        //[HttpPut("Add-Order-Details-Test")]
        //public async Task<IActionResult> CreateOrderDetailsTest(int orderID)
        //{
        //    var oOrder = await _context.OrdersTbls.Where(o => o.OrderId.Equals(orderID)).FirstOrDefaultAsync();
        //    List<CartDTO> cartDTOs = await _cart.GetUserCarts(oOrder.AccId.Value);
        //    List<OrderDetailsTbl> listOrder = new List<OrderDetailsTbl>();
        //    foreach (var cart in cartDTOs)
        //    {
        //        OrderDetailsTbl orderDetailsTbl = new OrderDetailsTbl
        //        {
        //            OrderId = orderID,
        //            ProductId = cart.ProductId,
        //            Quantity = cart.Quantity,
        //            TotalPrice = _normalFunctions.TotalMoneyOfItem(cart.ProductId, cart.Quantity)
        //        };
        //        Console.WriteLine(orderDetailsTbl);
        //        listOrder.Add(orderDetailsTbl);
        //    }
        //    // Nhap sau de tranh bi loi trong qua trinh tranfer cart, order.
        //    foreach (var order in listOrder)
        //    {
        //        _context.OrderDetailsTbls.Add(order);
        //    }

        //    await _context.SaveChangesAsync();
        //    return Ok(new { message = "Success", status = true });
        //}
    }
}
