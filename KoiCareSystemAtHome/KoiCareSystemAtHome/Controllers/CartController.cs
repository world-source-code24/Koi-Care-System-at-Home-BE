using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories;
using KoiCareSystemAtHome.Repositories.IRepositories;
using KoiCareSystemAtHome.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KoiCareSystemAtHome.Controllers
{
    public class CartController : Controller
    {
        private readonly IGenericRepository<CartDTO> _cartService;
        private readonly KoiCareSystemDbContext _context;
        private readonly ICartRepository _cartRepository;
        private readonly ICartDetailsRepository _cartDetailsRepository;
        public CartController(IGenericRepository<CartDTO> cartService, KoiCareSystemDbContext context
            , ICartRepository cartRepository, ICartDetailsRepository cartDetailsRepository)
        {
            _cartService = cartService;
            _context = context;
            _cartRepository = cartRepository;
            _cartDetailsRepository = cartDetailsRepository;
        }

        [HttpGet("Show-All-Carts")]
        public async Task<ActionResult<IEnumerable<CartDTO>>> ShowAllCart ()
        {
            var listCart = await _context.CartTbls.ToListAsync();
            if (listCart.Count == 0)
            {
                return NotFound("No one even try to buy so cart is empty");
            }
            else
            {
                return Ok(new { message = "success", listCart });
            }
    
        }

        [HttpGet("Show-All-Carts-From-User/{userID}")]
        public async Task<ActionResult<IEnumerable<CartDTO>>> ShowAllUserCarts (int userID)
        {
            bool checkUserExist = await _context.AccountTbls.AnyAsync(acc => acc.AccId == userID);
            if (!checkUserExist)
            {
                return NotFound("User didn't exist");
            }
            else
            {
                var listUserCart = await _context.CartTbls.Where(cart => cart.AccId == userID).ToListAsync();
                if (listUserCart.Count == 0)
                {
                    return NotFound("Cart is empty");
                }
                else
                {
                    return Ok(new { message = "success", listUserCart });
                }
               
            }           
        }

        [HttpGet("Get-Single-Cart-From-User/{userID}")]
        public async Task<ActionResult<CartDTO>> GetCart (int userID, int productID)
        {
            var getCart = await _context.CartTbls.FindAsync(userID, productID);
            return Ok(getCart);
        }

        [HttpGet("Get-Single-Cart-And-Details-From-User/(IN PROGRESS)")]
        public async Task<ActionResult<CartDetailsDTO>> GetFullInformation(int userID, int productID)
        {
            var getCart = await _context.CartTbls.FindAsync(userID, productID);
            var getCartDetails = _cartDetailsRepository.GetProductInfo(productID);
            return Ok((getCartDetails));
        }

        [HttpPost("Add-Carts-For-User/{userID}")]
        public async Task<ActionResult<CartDTO>> AddCart(int userID, CartDTO cartAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (isStockAvailable, stockMessage) = await _cartRepository.CheckStockAndProcessOrder(cartAdd.ProductId, cartAdd.Quantity);
            if (!isStockAvailable)
            {
                return BadRequest(new { status = false, message = stockMessage });
            }
            var cartTbl = new CartTbl
            {
                ProductId = cartAdd.ProductId,
                AccId = userID,
                Quantity = cartAdd.Quantity
            };

            _context.CartTbls.Add(cartTbl);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCart), new { status = true, message = "Cart added successfully" }, cartTbl);
        }


        [HttpPut ("Update-Cart-Quantity")]
        public async Task<ActionResult<CartDTO>> UpdateCart (CartDTO cartUpdate)
        {
            var oldCart = await _context.CartTbls
                .Where(c => c.ProductId == cartUpdate.ProductId && c.AccId == cartUpdate.AccId)
                .FirstOrDefaultAsync();
            if (oldCart == null)
            {
                return NotFound("Not found cart item");
            }
            else
            {
                var (isStockAvailable, stockMessage) = await _cartRepository.CheckStockAndProcessOrder(cartUpdate.ProductId, cartUpdate.Quantity);
                if (!isStockAvailable)
                {
                    return BadRequest(new { status = false, message = stockMessage });
                }
                oldCart.Quantity = cartUpdate.Quantity;
                await _context.SaveChangesAsync();
                return Ok(new {status = true, message = "Update success",  cartUpdate});
            }
        }

        [HttpDelete("Delete-Items")]
        public async Task<IActionResult> DeleteCart (int userID, int deletePoductID)
        {
            var cart = await _context.CartTbls.Where(c => c.AccId == userID && c.ProductId == deletePoductID).FirstOrDefaultAsync();
            if (cart == null)
            {
                return NotFound("Not found cart");
            }
            else
            {
                _context.CartTbls.Remove(cart);
                await _context.SaveChangesAsync();
                return Ok(new { status = true, message = "Delete success"});
            }
        }

        [HttpDelete("Delete-All--User-Carts")]
        public async Task<IActionResult> DeleteAllUserCarts (int userID)
        {
            var listCart = await _context.CartTbls.Where(c => c.AccId == userID).ToListAsync();
            if (listCart.Count == 0)
            {
                return NotFound("User cart is empty or not found");
            }
            else
            {
                _context.CartTbls.RemoveRange(listCart);
                await _context.SaveChangesAsync();
                return Ok(new { status = true, message = "Payment" });
            }
        }
    }
}
