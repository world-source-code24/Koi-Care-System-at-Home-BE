using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystemAtHome.Repositories
{
    public class ProductRepository : GenericRepository<ProductsTbl>, IProductRepository
    {
        private readonly KoicareathomeContext _context;
        public ProductRepository(KoicareathomeContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<ProductsTbl>> GetAllProductsAsync()
        {
            var results = await _context.ProductsTbls
                .Select(p => new ProductsTbl
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    Image = p.Image,
                    Category = p.Category,
                    ProductInfo = p.ProductInfo,
                    Status = p.Status,
                    ShopId = p.ShopId
                }).ToListAsync();
            return results;
        }
        public async Task<List<ProductsTbl>> GetAllByShopIdAsync(int shopId)
        {
            var results = await _context.ProductsTbls.Where(p => p.ShopId == shopId)
                .Select(p => new ProductsTbl
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    Image = p.Image,
                    Category = p.Category,
                    ProductInfo = p.ProductInfo,
                    Status = p.Status,
                    ShopId = p.ShopId
                }).ToListAsync();
            return results;
        }

        public async Task<ProductsTbl> GetByProductIdAsync(int id)
        {
            var result = await _context.ProductsTbls.Where(p => p.ProductId == id)
                .Select(p => new ProductsTbl
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    Image = p.Image,
                    Category = p.Category,
                    ProductInfo = p.ProductInfo,
                    Status = p.Status,
                    ShopId = p.ShopId
                }).FirstOrDefaultAsync();
            return result;
        }


        public async Task<List<ProductsTbl>> SearchByNameAsync(string name)
        {
            var results = await _context.ProductsTbls.Where(p => p.Name.Contains(name))
                .Select(p => new ProductsTbl
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    Image = p.Image,
                    Category = p.Category,
                    ProductInfo = p.ProductInfo,
                    Status = p.Status,
                    ShopId = p.ShopId
                }).ToListAsync();
            return results;
        }

        public async Task<int> GetTotalProduct()
        {
            return await _context.ProductsTbls.CountAsync(p => p.Status!=false);
        }

        public async Task<bool> ChangeStockProduct(int orderId, bool status)
        {
            var lOrderDetails = await _context.OrderDetailsTbls.Where(o => o.OrderId == orderId).ToListAsync();
            if (lOrderDetails.Count < 1) return false;
            foreach (var o in lOrderDetails)
            {
                var pProduct = await _context.ProductsTbls.FirstOrDefaultAsync(p => p.ProductId == o.ProductId);

                if (o.Quantity.HasValue && pProduct != null)
                { if (status) pProduct.Stock -= o.Quantity.Value;
                    else pProduct.Stock += o.Quantity.Value;
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
