using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories;
using KoiCareSystemAtHome.Repositories.IRepositories;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KoiCareSystemAtHome.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly KoicareathomeContext _context;
        private readonly INormalFunctionsRepository _normalFunctionsRepository;
        private readonly ICartRepository _cartRepository;
        private readonly AllEnum _aenum;
        private readonly IOrderRepository _orderRepository;

        public OrderController(KoicareathomeContext context, INormalFunctionsRepository normalFunctionsRepository, ICartRepository cartRepository, AllEnum aenum, IOrderRepository orderRepository)
        {
            _context = context;
            _normalFunctionsRepository = normalFunctionsRepository;
            _cartRepository = cartRepository;
            _aenum = aenum;
            _orderRepository = orderRepository;
        }


        [HttpGet("/api/Get-All-User-Order")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrder(int userId)
        {
            var order = await _context.OrdersTbls.Where(o => o.AccId == userId).ToListAsync();
            if (order == null)
            {
                return NotFound("Not found order");
            }
            else
            {
                return Ok(new { message = "success", status = true, order });
            }
        }

        [HttpGet("GetAll/{accId}")]
        public async Task<IActionResult> GetOrdersByAccId(int accId)
        {
            var orders = await _orderRepository.GetOrdersByAccId(accId);
            if (orders.IsNullOrEmpty())
            {
                return NotFound("No orders available!!");
            }
            return Ok(new {success =  true, orders = orders});

        }

        [HttpGet("/api/Get-Order")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrder(int orderId)
        {
            var order = await _context.OrdersTbls.Where(o => o.OrderId == orderId).FirstOrDefaultAsync();
            if (order == null)
            {
                return NotFound("Not found order");
            }
            else
            {
                return Ok(new { message = "success", status = true, order });
            }
        }

        //[HttpGet("/api/Get-Order")]
        //public async Task<IActionResult> GetOrder(int orderId)
        //{
        //    var order = await _orderRepository.GetOrder(orderId);
        //    if (order == null)
        //    {
        //        return NotFound("Not found order");
        //    }
        //    else
        //    {
        //        return Ok(new { message = "success", status = true, order });
        //    }
        //}


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

        [HttpGet("/api/Get-Order-By-OrderStatus")]
        public async Task<IActionResult> GetOrdersByOrderStatus(int status, int accId)
        {
            List<OrdersTbl> orders = new List<OrdersTbl>();
            switch (status)
            {
                //1: Peding
                //2: Processing
                //3: Ship
                //4: Ship Complete
                //5: Cancel
                case 1:
                    orders = _context.OrdersTbls.Where(o => o.AccId == accId && o.StatusOrder.ToLower().Equals("pending")).ToList();
                    break;
                case 2:
                    orders = _context.OrdersTbls.Where(o => o.AccId == accId && o.StatusOrder.ToLower().Equals("processing")).ToList();
                    break;
                case 3:
                    orders = _context.OrdersTbls.Where(o => o.AccId == accId && o.StatusOrder.ToLower().Equals("shiping")).ToList();
                    break;
                case 4:
                    orders = _context.OrdersTbls.Where(o => o.AccId == accId && o.StatusOrder.ToLower().Equals("shipcompleted")).ToList();
                    break;
                case 5:
                    orders = _context.OrdersTbls.Where(o => o.AccId == accId &&  o.StatusOrder.ToLower().Equals("completed")).ToList();
                    break;
                case 6:
                    orders = _context.OrdersTbls.Where(o => o.AccId == accId && o.StatusOrder.ToLower().Equals("cancelled")).ToList();
                    break;

            }
            return Ok(new {status = true, orders = orders});
        }

        [HttpPost("/api/Create-Order-And-Calculate-Money")]
        public async Task<IActionResult> CreateOrderAndMoney(int accID)
        {
            try
            {
                var totalCart = await _cartRepository.GetUserCarts(accID);
                if (totalCart == null) return BadRequest(new { message = "Cart empty" });
                var totalAmount = await _normalFunctionsRepository.TotalMoneyOfCarts(totalCart);                
                var order = new OrdersTbl
                {
                    AccId = accID,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    StatusOrder = AllEnum.OrderStatus.Processing.ToString(),
                    StatusPayment = AllEnum.StatusPayment.Unpaid.ToString(),
                    TotalAmount = totalAmount
                };
                _context.OrdersTbls.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in totalCart)
                {
                    var product = await _context.ProductsTbls.FindAsync(item.ProductId);
                    var orderDetails = new OrderDetailsTbl
                    {
                        OrderId = order.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        TotalPrice = item.Quantity * product.Price
                    };
                    _context.OrderDetailsTbls.Add(orderDetails);
                }
                await _context.SaveChangesAsync();
                return Ok(new { status = true, orderId = order.OrderId });
            }
            catch (Exception n)
            {
                return BadRequest(n.Message);
            }
        }

        //[HttpPut("/api/Update-Status-Payment")]
        //public async Task<IActionResult> UpdatePaidSuccess(int accID, int orderID)
        //{ 
        //    var order = await _context.OrdersTbls
        //        .Where(order => order.AccId == accID && order.OrderId == orderID)
        //        .FirstOrDefaultAsync();
        //    if (order == null) return NotFound("Not found this order");
        //    order.StatusPayment = AllEnum.StatusPayment.Paid.ToString();
        //    await _context.SaveChangesAsync();
        //    return Ok(new { status = true, message = "Payment status updated to Paid." });
        //}

        [HttpPut("/api/Update-Status-Payment")]
        public async Task<IActionResult> UpdatePaidSuccessLatest(int orderID)
        {
            var order = await _orderRepository.GetOrder(orderID);
            if (order == null) return NotFound("Not found this order");
            order.StatusPayment = "Paid";
             _context.OrdersTbls.Update(order);
            await _context.SaveChangesAsync();
            return Ok(new { status = true, message = "Payment status updated to Paid.", payment = order.StatusPayment });
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
            
        [HttpPut("/api/Update-OrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus(int orderID, int status )
        {
            bool bCheck = await _orderRepository.SetOrderStatus(orderID, status);
            if (bCheck== false) return NotFound("Not found this order");    
            var order = await _orderRepository.GetOrder(orderID);
            return Ok(new { status = true, message = "Order status updated success.", orderStatus =  order.StatusOrder});
        }


    }
}
