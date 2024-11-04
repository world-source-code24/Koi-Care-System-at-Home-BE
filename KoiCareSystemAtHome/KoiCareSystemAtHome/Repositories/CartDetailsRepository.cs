using KoiCareSystemAtHome.Controllers;
using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KoiCareSystemAtHome.Repositories
{
    public class CartDetailsRepository : ICartDetailsRepository
    {
        private readonly IGenericRepository<ProductsTbl> _productsTblRepository;
        private readonly KoicareathomeContext _context;
        public CartDetailsRepository(IGenericRepository<ProductsTbl> genericRepository, KoicareathomeContext context)
        {
            _productsTblRepository = genericRepository;
            _context = context;
        }
        public async Task<CartDetailsDTO> GetProductInfo(int productId)
        {
            var product =await _context.ProductsTbls.FindAsync(productId);
            //if (product == null) return null;
            var cartDetails = new CartDetailsDTO
            {
                Price = product.Price,
                ProductName = product.Name,
                Quantity = product.Stock,
                Category = product.Category,
            };
            return cartDetails;
        }
        public async Task<CartDetailsDTO> GetCardDetails(int productId, int quantity)
        {
            var product = await _context.ProductsTbls.FindAsync(productId);
            //if (product == null) return null;
            var cartDetails = new CartDetailsDTO
            {
                Price = product.Price * quantity,
                ProductName = product.Name,
                Quantity = product.Stock,
                Category = product.Category,
            };
            return cartDetails;
        }
    }
}
