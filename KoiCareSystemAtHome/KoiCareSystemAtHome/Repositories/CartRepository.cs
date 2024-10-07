using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KoiCareSystemAtHome.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly KoiCareSystemDbContext _context;

        public CartRepository(KoiCareSystemDbContext context)
        {
            _context = context;
        }

        public async Task<(bool IsSuccess, string Message)> CheckStockAndProcessOrder(int productId, int requestedQuantity)
        {
            var stock = await _context.ProductsTbls
                .Where(p => p.ProductId == productId)
                .Select(p => p.Stock)
                .FirstOrDefaultAsync();

            if (stock == 0)
            {
                return (false, "Product not found or out of stock.");
            }

            if (stock < requestedQuantity)
            {
                return (false, "Insufficient stock available.");
            }

            // Process order if stock is sufficient
            return (true, "Stock is sufficient, processing order.");
        }
    }
}
