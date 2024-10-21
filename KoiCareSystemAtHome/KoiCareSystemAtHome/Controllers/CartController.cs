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

        [HttpGet("/api/Show-All-Carts")]
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

        [HttpGet("/api/Show-All-Carts-From-User/{userID}")]
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

        //[HttpGet("Get-Single-Cart-From-User/{userID}")]
        //public async Task<ActionResult<CartDTO>> GetCart (int userID, int productID)
        //{
        //    var getCart = await _context.CartTbls.FindAsync(userID, productID);
        //    return Ok(getCart);
        //}

        [HttpGet("/api/Get-Single-Cart-And-Details-From-User/")]
        public async Task<ActionResult<(CartDTO,CartDetailsDTO)>> GetCartInformation(int userID, int productID)
        {
            var getCart = await _context.CartTbls.FindAsync(userID, productID);
            var getCartDetails = await _cartDetailsRepository.GetProductInfo(productID);
            if (getCart == null || getCartDetails == null)
            {
                return NotFound("Cart or Product details not found.");
            }
            var cartDTO = new CartDTO
            {
                AccId = userID,
                ProductId = productID,
                Quantity = getCart.Quantity
            };
            return Ok(new { Cart = cartDTO, Details = getCartDetails });
        }

        [HttpGet("/api/Get-All-Cart-And-Details-From-User (For display)")]
        public async Task<ActionResult<List<FullCartDetailDTO>>> GetFullInformation(int userID)
        {
            var cartResult = await _cartRepository.GetUserCarts(userID);      
            if (cartResult == null) return NotFound("Object is null");
            var fullCartDetailsList = new List<FullCartDetailDTO>();

            foreach (var cart in cartResult)
            {
                var cartDTO = new CartDTO
                {
                    AccId = userID,
                    ProductId = cart.ProductId,
                    Quantity = cart.Quantity
                };

                var cartDetail = await _cartDetailsRepository.GetProductInfo(cart.ProductId);
                if (cartDetail != null)
                {
                    fullCartDetailsList.Add(new FullCartDetailDTO
                    {
                        Cart = cartDTO,
                        CartDetails = cartDetail
                    });
                }
            }
            return Ok( fullCartDetailsList );
        }



        [HttpPost("/api/Add-Carts-For-User/")]
        public async Task<ActionResult<CartDTO>> AddCart(CartDTO cartAdd)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check stock availability and process the order
            var (isStockAvailable, stockMessage) = await _cartRepository.CheckStockAndProcessOrder(cartAdd.ProductId, cartAdd.Quantity);
            if (!isStockAvailable)
            {
                return BadRequest(new { status = false, message = stockMessage });
            }

            // Create a new cart entity
            var cartTbl = new CartTbl
            {
                ProductId = cartAdd.ProductId,
                AccId = cartAdd.AccId,
                Quantity = cartAdd.Quantity
            };

            // Add the new cart to the database
            _context.CartTbls.Add(cartTbl);
            await _context.SaveChangesAsync();

            // Return created response with details about the newly added cart
            return CreatedAtAction(nameof(AddCart), new { status = true, message = "Cart added successfully" }, cartTbl);
        }



        [HttpPut ("/api/Update-Cart-Quantity")]
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

        [HttpDelete("/api/Delete-Items")]
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

        [HttpDelete("/api/Delete-All-User-Carts")]
        public async Task<IActionResult> DeleteAllUserCarts(int userID)
        {
            bool check =await _cartRepository.DeleteAllCart(userID);
            if (!check)
            {
                return NotFound("User cart is empty or not found");
            }
            else
            {
                await _context.SaveChangesAsync();
                return Ok(new { status = true, message = "Payment" });
            }
        }
    }
}
