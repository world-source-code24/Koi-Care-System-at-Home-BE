using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Formats.Asn1;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopRepository _shopRepository;
        private readonly IProductRepository _productRepository;
        public ShopController(IShopRepository shopRepository, IProductRepository productRepository)
        {
            _shopRepository = shopRepository;
            _productRepository = productRepository;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllShops()
        {
            var shops =  await _shopRepository.GetAllShopsAsync();
            int totalShops = await _shopRepository.GetTotalShops();
            return Ok(new {success = true, shops = shops, total = totalShops});
        }

        [HttpGet("get-byId{shopId}")]
        public async Task<IActionResult> GetByShopId(int shopId)
        {
            try
            {
                var shop = await _shopRepository.GetByShopIdAsync(shopId);
                if (shop == null)
                {
                    return NotFound("No shop available!!");
                }
                return Ok(new {success = true, shop = shop});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateShop(ShopDTO shop)
        {
            try
            {
                if (shop.Name.IsNullOrEmpty() || shop.Phone.IsNullOrEmpty())
                {
                    return BadRequest("Can not create shop!!");
                }
                var newShop = new ShopsTbl
                {
                    Name = shop.Name,
                    Address = shop.Address,
                    Phone = shop.Phone,
                };
                await _shopRepository.AddAsync(newShop);
                return CreatedAtAction(nameof(GetByShopId), new {shopId = newShop.ShopId}, newShop);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update{shopId}")]
        public async Task<IActionResult> UpdateShop(int shopId, ShopDTO shop)
        {
            try
            {
                if (shop.Name.IsNullOrEmpty() || shop.Phone.IsNullOrEmpty())
                {
                    return BadRequest("Can not update to empty data!!");
                }
                var updateShop = await _shopRepository.GetByShopIdAsync(shopId);
                if (updateShop == null)
                {
                    return NotFound("No shop available!!");
                }
                updateShop.Address = shop.Address;
                updateShop.Phone = shop.Phone;
                updateShop.Name = shop.Name;
                await _shopRepository.UpdateAsync(updateShop);
                return Ok(new {success = true, message = "Update successfully!!", shop = updateShop});
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Delte shop => shopId in productTable set to null
        //Product status belongs to shop  set to false
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteShop(int shopId)
        {
            try
            {
                var shop = await _shopRepository.GetByIdAsync(shopId);
                if (shop == null)
                {
                    return BadRequest("No shop available!!");
                }
                var productList = await _productRepository.GetAllByShopIdAsync(shopId);
                productList?.ForEach(product =>
                    {
                        product.Status = false;
                    });
                await _shopRepository.DeleteAsync(shopId);
                return Ok(new { success = true, message = "Delete successfully!!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
